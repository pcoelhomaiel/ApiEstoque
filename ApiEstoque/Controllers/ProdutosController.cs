using ApiEstoque.Data;
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
    public class ProdutosController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public ProdutosController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/produtos
        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos
                .Where(p => !p.Excluido)
                .ToListAsync();
        }

        // GET: api/produtos/5
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null || produto.Excluido)
            {
                return NotFound(new { message = "Produto não encontrado" });
            }

            return produto;
        }

        // POST: api/produtos
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
        {
            if (produto == null)
                return BadRequest(new { message = "Dados inválidos" });

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // PUT: api/produtos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
                return BadRequest(new { message = "ID da URL não corresponde ao ID do objeto" });

            if (!_context.Produtos.Any(p => p.Id == id))
                return NotFound(new { message = "Produto não encontrado" });

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Erro ao atualizar o produto" });
            }

            return NoContent();
        }

        // DELETE (Soft Delete): api/produtos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null || produto.Excluido)
                return NotFound(new { message = "Produto não encontrado" });

            produto.Excluido = true;
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
