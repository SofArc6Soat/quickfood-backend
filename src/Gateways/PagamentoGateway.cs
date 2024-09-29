using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

namespace Gateways
{
    public class PagamentoGateway(IPagamentoRepository pagamentoRepository) : IPagamentoGateway
    {
        public async Task<bool> CadastrarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken)
        {
            var pagementoDto = new PagamentoDb
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status.ToString(),
                QrCodePix = pagamento.QrCodePix,
                Valor = pagamento.Valor,
                DataPagamento = pagamento.DataPagamento
            };

            await pagamentoRepository.InsertAsync(pagementoDto, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> NotificarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken)
        {
            var pagementoDto = new PagamentoDb
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status.ToString(),
                QrCodePix = pagamento.QrCodePix,
                Valor = pagamento.Valor,
                DataPagamento = pagamento.DataPagamento
            };

            await pagamentoRepository.UpdateAsync(pagementoDto, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public Pagamento? ObterPagamentoPorPedido(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pagamentoDto = pagamentoRepository.Find(e => e.PedidoId == pedidoId, cancellationToken).FirstOrDefault();

            if (pagamentoDto is null)
            {
                return null;
            }

            _ = Enum.TryParse(pagamentoDto.Status, out StatusPagamento statusPagamento);

            return new Pagamento(pagamentoDto.Id, pagamentoDto.PedidoId, statusPagamento, pagamentoDto.Valor, pagamentoDto.QrCodePix, pagamentoDto.DataPagamento);
        }

        public string GerarQrCodePixGatewayPagamento(Pagamento pagamento)
        {
            // Integração com gateway de pagamento e geração QR Code do PIX

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, 100)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken) =>
            await pagamentoRepository.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
        public Pagamento? ObterPagamentoPorPedido(Guid pedidoId) => throw new NotImplementedException();
    }
}
