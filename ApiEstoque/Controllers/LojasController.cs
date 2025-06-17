using ApiEstoque.Data;
using ApiEstoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEstoque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LojasController : ControllerBase
    {
        private readonly AplicacaoDbContext _context;

        public LojasController(AplicacaoDbContext context)
        {
            _context = context;
        }

        // GET: api/lojas
        [Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loja>>> GetLojas()
        {
            return await _context.Lojas
                .Where(l => !l.Excluido)
                .ToListAsync();
        }

        // GET: api/lojas/5
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Loja>> GetLoja(int id)
        {
            var loja = await _context.Lojas.FindAsync(id);

            if (loja == null || loja.Excluido)
                return NotFound(new { message = "Loja não encontrada" });

            return loja;
        }

        // POST: api/lojas
        [HttpPost]
        public async Task<ActionResult<Loja>> PostLoja([FromBody] Loja loja)
        {
            if (loja == null)
                return BadRequest(new { message = "Dados inválidos" });

            _context.Lojas.Add(loja);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoja), new { id = loja.Id }, loja);
        }

        // PUT: api/lojas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoja(int id, [FromBody] Loja loja)
        {
            if (id != loja.Id)
                return BadRequest(new { message = "ID da URL não corresponde ao ID do objeto" });

            if (!_context.Lojas.Any(l => l.Id == id))
                return NotFound(new { message = "Loja não encontrada" });

            _context.Entry(loja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Erro ao atualizar a loja" });
            }

            return NoContent();
        }

        // DELETE (Soft Delete): api/lojas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoja(int id)
        {
            var loja = await _context.Lojas.FindAsync(id);

            if (loja == null || loja.Excluido)
                return NotFound(new { message = "Loja não encontrada" });

            loja.Excluido = true;
            _context.Lojas.Update(loja);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
