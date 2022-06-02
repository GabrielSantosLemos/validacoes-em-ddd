namespace Dominio
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }

        public Produto(string nome, int id = default)
        {
            Id = id;
            Nome = nome;
        }
    }
}
