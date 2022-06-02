using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers.Pedidos
{
    public class InserirPedidoInputModel
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public IEnumerable<ItemInputModel> Itens { get; set; }
    }
}
