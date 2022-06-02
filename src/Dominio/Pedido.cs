using CSharpFunctionalExtensions;

namespace Dominio
{
    public class Pedido
    {
        private readonly List<ItemDoPedido> _itens = new();

        public int Id { get; private set; }
        public int ClienteId { get; private set; }

        public Pedido(int clienteId, int id = default)
        {
            Id = id;
            ClienteId = clienteId;
        }

        public Result AddItem(int produtoId, int quantidade)
        {
            return Result.FailureIf(_itens.Any(x => x.ProdutoId == produtoId), "Produto já adicionado")
                  .Tap(() =>
                  {
                      _itens.Add(new ItemDoPedido(produtoId, quantidade));
                  })
                  .Finally(_ => _);
        }
    }
}
