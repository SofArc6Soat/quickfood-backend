using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PedidoDto : Entity
    {
        public int NumeroPedido { get; set; }
        public Guid? ClienteId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public DateTime DataPedido { get; set; }

        public List<PedidoItemDto> Itens { get; set; }

        public PedidoDto() => Itens = [];
    }
}
