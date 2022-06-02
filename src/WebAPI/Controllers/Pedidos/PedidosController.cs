using CSharpFunctionalExtensions;
using Dominio;
using Infraestrutura;
using Microsoft.AspNetCore.Mvc;
using WebAPI._Core;

namespace WebAPI.Controllers.Pedidos
{
    [Route("api/pedidos")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly Context _context = new();

        [HttpPost]
        public IActionResult Inserir([FromBody] InserirPedidoInputModel input)
        {
            if (!_context.Cliente.Any(x => x.Id == input.ClienteId))
                ModelState.AddModelError(nameof(input.ClienteId), "Cliente não existe.");

            if (_context.Cliente.Any(x => x.Id == input.ClienteId && x.Negativado))
                ModelState.AddModelError(nameof(input.ClienteId), "Cliente negativado.");

            Pedido pedido = new(input.ClienteId);

            foreach ((ItemInputModel itemInput, int indice) in input.Itens.WithIndex(true))
            {
                string prop = $"{nameof(input.Itens)}[{indice}].{nameof(itemInput.ProdutoId)}";

                if (!_context.Produto.Any(x => x.Id == itemInput.ProdutoId))
                    ModelState.AddModelError(prop, "Produto não existe.");

                Result addItem = pedido.AddItem(itemInput.ProdutoId, itemInput.Quantidade);
                if(addItem.IsFailure)
                    ModelState.AddModelError(prop, addItem.Error);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Pedido.Add(pedido);

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult Buscar(int id)
        {
            Pedido? pedido = _context.Pedido.FirstOrDefault(x => x.Id == id);
            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }
    }
}
