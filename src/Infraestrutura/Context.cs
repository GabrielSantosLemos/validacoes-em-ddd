using Dominio;

namespace Infraestrutura
{
    public class Context
    {
        public List<Pedido> Pedido { get; set; } = new()
        {
            new Pedido(1, 1),
            new Pedido(2, 2)
        };

        public List<Cliente> Cliente { get; set; } = new()
        {
            new Cliente(Nome.Criar("GSL").Value, Email.Criar("gsl@gsl.com").Value, true, 1),
            new Cliente(Nome.Criar("ALM").Value, Email.Criar("alm@alm.com").Value, false, 2),
        };

        public List<Produto> Produto { get; set; } = new()
        {
            new Produto("KJY220", 1),
            new Produto("FDR586", 2),
            new Produto("TREW56", 3),
            new Produto("UNBK45", 8),
        };
    }
}