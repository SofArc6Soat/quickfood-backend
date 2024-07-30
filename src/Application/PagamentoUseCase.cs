using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Infra.Repositories;

namespace UseCases
{
    public class PagamentoUseCase(IPedidoGateway pedidoGateway, IPagamentoGateway pagamentoGateway, IPagamentoRepository pagamentoRepository, INotificador notificador) : BaseUseCase(notificador), IPagamentoUseCase
    {
        public async Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pedido = await pedidoGateway.ObterPedidoAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            if (!pedido.EfetuarCheckout())
            {
                Notificar("Não foi possível realizar o checkout do pedido.");
                return false;
            }

            var pagamentoExistente = pagamentoGateway.ObterPagamentoPorPedido(pedidoId, cancellationToken);

            if (pagamentoExistente is not null)
            {
                Notificar("Pagamento já existente para o pedido, aguarde a confirmação do seu Pix.");
                return false;
            }

            if (await pedidoGateway.AtualizarPedidoAsync(pedido, cancellationToken))
            {
                var pagamento = new Pagamento(pedidoId, pedido.ValorTotal);
                pagamento.GerarQrCodePix();
                pagamento.AlterarStatusPagamentoParaPendente();

                return await pagamentoGateway.CadastrarPagamentoAsync(pagamento, cancellationToken);
            }

            return false;
        }

        public async Task<bool> NotificarPagamentoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pagamento = pagamentoGateway.ObterPagamentoPorPedido(pedidoId, cancellationToken);

            if (pagamento is null)
            {
                Notificar($"Pagamento inexistente para o {pedidoId}, verifique se o pedido foi efetuado corretamente");
                return false;
            }

            var pedido = await pedidoGateway.ObterPedidoAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            pagamento.AlterarStatusPagamentoParaPago();

            pedido.AlterarStatusParaRecebibo();

            if (await pedidoGateway.AtualizarPedidoAsync(pedido, cancellationToken))
            {
                await pagamentoGateway.CadastrarPagamentoAsync(pagamento, cancellationToken);
                return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
            }

            return false;
        }

        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken) =>
            await pagamentoRepository.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
    }
}
