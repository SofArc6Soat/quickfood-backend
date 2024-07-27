namespace Domain.Entities
{
    public class PedidoListaItens
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public PedidoListaItens(Guid produtoId, int quantidade)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
    }
}
