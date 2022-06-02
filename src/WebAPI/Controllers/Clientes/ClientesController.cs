using Dominio;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Clientes
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly Context _context = new();

        [HttpPost]
        public IActionResult Inserir([FromBody] ClienteInputModel input)
        {
            if(_context.Cliente.Any(x => x.Email.Equals(input.Email)))
                ModelState.AddModelError(nameof(input.Email), "E-Mail já existente.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Cliente cliente = new(
                Nome.Criar(input.Nome).Value, 
                Email.Criar(input.Email).Value
            );

            _context.Cliente.Add(cliente);

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult Buscar(int id)
        {
            Cliente? cliente = _context.Cliente.FirstOrDefault(x => x.Id == id);

            return Ok(cliente);
        }
    }
}
