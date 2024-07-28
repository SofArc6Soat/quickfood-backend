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
        public DateTime DataPagamento { get; private set; }

        public Pagamento(Guid pedidoId, decimal valor)
        {
            PedidoId = pedidoId;
            Valor = valor;
            DataPagamento = DateTime.Now;
        }

        public Pagamento(Guid id, Guid pedidoId, StatusPagamento status, decimal valor, string? qrCodePix, DateTime dataCriacao)
        {
            Id = id;
            PedidoId = pedidoId;
            Status = status;
            Valor = valor;
            QrCodePix = qrCodePix;
            DataPagamento = dataCriacao;
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
