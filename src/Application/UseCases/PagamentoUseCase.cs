using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;

namespace UseCases.UseCases
{
    public class PagamentoUseCase(IPedidoRepository pedidoRepository, IPagamentoRepository pagamentoRepository, INotificador notificador) : BaseUseCase(notificador), IPagamentoUseCase
    {
        public async Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pedido = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

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

            var pagamentoExistente = pagamentoRepository.Find(e => e.PedidoId == pedido.Id).Any();

            if (pagamentoExistente)
            {
                Notificar("Pagamento já existente para o pedido, aguarde a confirmação do seu Pix.");
                return false;
            }

            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            var pagamento = new Pagamento(pedidoId, pedido.ValorTotal);
            pagamento.GerarQrCodePix();
            pagamento.AlterarStatusPagamentoParaPendente();

            await pagamentoRepository.InsertAsync(pagamento, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> NotificarPagamentoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pagamentoExistente = pagamentoRepository.Find(e => e.PedidoId == pedidoId).FirstOrDefault();

            if (pagamentoExistente is null)
            {
                Notificar($"Pagamento inexistente para o {pedidoId}, verifique se o pedido foi efetuado corretamente");
                return false;
            }

            pagamentoExistente.AlterarStatusPagamentoParaPago();
            await pagamentoRepository.UpdateAsync(pagamentoExistente, cancellationToken);

            var pedido = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            pedido.AlterarStatusParaRecebibo();

            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken) =>
            await pagamentoRepository.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
    }
}
