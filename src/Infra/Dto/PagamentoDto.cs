using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PagamentoDto : Entity
    {
        public Guid PedidoId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string? QrCodePix { get; set; }
        public DateTime DataPagamento { get; set; }

#pragma warning disable CS8618
        public PedidoDto Pedido { get; set; }
#pragma warning restore CS8618
    }
}
