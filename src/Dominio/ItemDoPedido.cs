namespace Dominio
{
    public class ItemDoPedido
    {
        public int Id { get; private set; }
        public int ProdutoId { get; private set; }
        public int Quantidade { get; private set; }

        public ItemDoPedido(int produtoId, int quantidade, int id = default)
        {
            Id = id;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
    }
}
