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
            Result<Email> email = Email.Criar(input.Email);
            Result<Nome> nome = Nome.Criar(input.Nome);

            if (email.IsFailure)
                ModelState.AddModelError("Email", email.Error);
            if (nome.IsFailure)
                ModelState.AddModelError("Nome", nome.Error);

            return NoContent();
        }
    }
}
