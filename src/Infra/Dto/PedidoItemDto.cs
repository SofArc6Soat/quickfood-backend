using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoItemDto : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoDto Pedido { get; set; }

        public PedidoItemDto(Guid id, Guid pedidoId, Guid produtoId, int quantidade, decimal valorUnitario)
        {
            Id = id;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }
    }
}
