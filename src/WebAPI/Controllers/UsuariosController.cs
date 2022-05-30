using CSharpFunctionalExtensions;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IEnumerable<Usuario> _usuarioRepository = new Usuario[] { Usuario.Criar("GSL", "gsl@gsl.com").Value };

        [HttpPost]
        public IActionResult Inserir([FromBody] UsuarioInputModel input)
        {
            bool usuarioExiste = _usuarioRepository.Any(x => x.Email == input.Email);
            if (usuarioExiste)
                return BadRequest("E-mail já existente.");

            Result<Usuario> criar = Usuario.Criar(input.Nome, input.Email);

            if (criar.IsFailure)
                return BadRequest(criar.Error);

            return NoContent();
        }

        [HttpPost("id")]
        public IActionResult AtualizarEmail(string email, int id)
        {
            bool usuarioExiste = _usuarioRepository.Any(x => x.Email == email);
            if (usuarioExiste)
                return BadRequest("E-mail já existente.");

            Usuario? usuario = _usuarioRepository.FirstOrDefault(x => x.Id == id);
            if (usuario is null)
                return NotFound($"Usuário id: {id} não existe.");

            Result atualizarEmail = usuario.AtualizarEmail(email);

            if (atualizarEmail.IsFailure)
                return BadRequest(atualizarEmail.Error);

            return NoContent();
        }
    }
}
