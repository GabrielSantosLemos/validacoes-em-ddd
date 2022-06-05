using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Produtos
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        [HttpPost]
        public IActionResult Inserir([FromBody] ProdutoInputModel input)
        {
            return NoContent();
        }
    }
}
