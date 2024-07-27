using Core.Domain.Entities;

namespace Infra.Dto
{
    public class PagamentoDto : Entity
    {
        public Guid PedidoId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public decimal Valor { get; private set; }
        public string? QrCodePix { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public PedidoDto Pedido { get; set; }

        public PagamentoDto(Guid id, Guid pedidoId, string status, decimal valor, string? qrCodePix, DateTime dataCriacao)
        {
            Id = id;
            PedidoId = pedidoId;
            Status = status;
            Valor = valor;
            QrCodePix = qrCodePix;
            DataCriacao = dataCriacao;
        }
    }
}
