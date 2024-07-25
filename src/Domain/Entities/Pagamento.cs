using Core.Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid PedidoId { get; private set; }
        public StatusPagamento Status { get; private set; }
        public decimal Valor { get; private set; }
        public string? QrCodePix { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public Pedido Pedido { get; set; }

        public Pagamento(Guid pedidoId, decimal valor)
        {
            PedidoId = pedidoId;
            Valor = valor;
            DataCriacao = DateTime.Now;
        }

        public void GerarQrCodePix()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            QrCodePix = new string(
                Enumerable.Repeat(chars, 100)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        public void AlterarStatusPagamentoParaPendente() =>
            Status = StatusPagamento.Pendente;

        public void AlterarStatusPagamentoParaPago() =>
            Status = StatusPagamento.Pago;
    }
}
