using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers.Pedidos
{
    public class ItemInputModel
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int Quantidade { get; set; }
    }
}
