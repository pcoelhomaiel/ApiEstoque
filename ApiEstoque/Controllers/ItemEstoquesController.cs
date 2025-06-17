using ApiEstoque.Data;
using ApiEstoque.DTOs;
using ApiEstoque.Models;
using ApiEstoque.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEstoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemEstoquesController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public ItemEstoquesController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/ItemEstoques
        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemEstoque>>> GetItemEstoques()
        {
            return await _context.ItemEstoques
                .Include(i => i.Loja)
                .Include(i => i.Produto)
                .ToListAsync();
        }

        // GET: api/ItemEstoques/5/2
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{idProduto}/{idLoja}")]
        public async Task<ActionResult<ItemEstoque>> GetItemEstoque(int idProduto, int idLoja)
        {
            var itemEstoque = await _context.ItemEstoques
                .Include(i => i.Loja)
                .Include(i => i.Produto)
                .FirstOrDefaultAsync(e => e.IdProduto == idProduto && e.IdLoja == idLoja);

            if (itemEstoque == null)
            {
                return NotFound();
            }

            return itemEstoque;
        }

        // SOCORRO
        [HttpPost]
        public async Task<ActionResult<ItemEstoque>> PostItemEstoque(ItemEstoqueCreateDto dto)
        {
            var itemEstoque = new ItemEstoque
            {
                IdProduto = dto.IdProduto,
                IdLoja = dto.IdLoja,
                Quantidade = dto.Quantidade
            };

            _context.ItemEstoques.Add(itemEstoque);
            await _context.SaveChangesAsync();

            // Recarrega com Include para Produto e Loja
            var itemEstoqueComIncludes = await _context.ItemEstoques
                .Include(i => i.Produto)
                .Include(i => i.Loja)
                .FirstOrDefaultAsync(i => i.IdProduto == itemEstoque.IdProduto && i.IdLoja == itemEstoque.IdLoja);

            return CreatedAtAction(nameof(GetItemEstoque), new { idProduto = itemEstoque.IdProduto, idLoja = itemEstoque.IdLoja }, itemEstoqueComIncludes);
        }


        // PUT: api/ItemEstoques/5/2
        [HttpPut("{idProduto}/{idLoja}")]
        public async Task<IActionResult> PutItemEstoque(int idProduto, int idLoja, [FromBody] ItemEstoqueCreateDto dto)
        {
            if (idProduto != dto.IdProduto || idLoja != dto.IdLoja)
            {
                return BadRequest("IDs do URL e do corpo não coincidem.");
            }

            var itemEstoque = await _context.ItemEstoques.FindAsync(idProduto, idLoja);

            if (itemEstoque == null)
            {
                return NotFound();
            }

            itemEstoque.Quantidade = dto.Quantidade;

            try
            {
                _context.Entry(itemEstoque).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemEstoqueExists(idProduto, idLoja))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ItemEstoques/5/2
        [HttpDelete("{idProduto}/{idLoja}")]
        public async Task<IActionResult> DeleteItemEstoque(int idProduto, int idLoja)
        {
            var itemEstoque = await _context.ItemEstoques.FindAsync(idProduto, idLoja);
            if (itemEstoque == null)
            {
                return NotFound();
            }

            _context.ItemEstoques.Remove(itemEstoque);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemEstoqueExists(int idProduto, int idLoja)
        {
            return _context.ItemEstoques.Any(e => e.IdProduto == idProduto && e.IdLoja == idLoja);
        }
    }
}
