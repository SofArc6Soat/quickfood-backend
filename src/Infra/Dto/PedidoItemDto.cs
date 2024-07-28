using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoItemDto : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }

        public PedidoDto Pedido { get; set; } = new PedidoDto();
    }
}
