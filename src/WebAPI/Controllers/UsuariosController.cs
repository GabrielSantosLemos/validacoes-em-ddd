using CSharpFunctionalExtensions;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IEnumerable<Usuario> _usuarioRepository = new Usuario[] { new Usuario(Nome.Criar("GSL").Value, Email.Criar("gsl@gsl.com").Value) };

        [HttpPost]
        public IActionResult Inserir([FromBody] UsuarioInputModel input)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Result<Email> email = Email.Criar(input.Email);
            Result<Nome> nome = Nome.Criar(input.Nome);

            if (email.IsFailure)
                ModelState.AddModelError("Email", email.Error);
            if (nome.IsFailure)
                ModelState.AddModelError("Nome", nome.Error);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Usuario usuario = new(nome.Value, email.Value);

            _usuarioRepository = _usuarioRepository.Append(usuario);

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult Buscar(int id)
        {
            Maybe<Usuario?> usuario = _usuarioRepository.FirstOrDefault(x => x.Id == id);

            return Ok(usuario);
        }
    }
}
