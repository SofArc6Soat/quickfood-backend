using Application.Models.Request;
using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace Application.UseCases
{
    public class PedidoUseCase(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IPedidoUseCase
    {
        public async Task<bool> CadastrarPedidoAsync(PedidoRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var pedidoExistente = await pedidoRepository.FindByIdAsync(request.PedidoId, cancellationToken);

            if (pedidoExistente is not null)
            {
                Notificar("Pedido já existente");
                return false;
            }

            var pedido = new Pedido(request.PedidoId, request.ClienteId);

            if (!ExecutarValidacao(new ValidarPedido(), pedido))
            {
                return false;
            }

            foreach (var item in request.Items)
            {
                var produto = await produtoRepository.FindByIdAsync(item.ProdutoId, cancellationToken);

                if (produto is null)
                {
                    Notificar($"Produto {item.ProdutoId} não encontrado.");
                }
                else
                {
                    pedido.AdicionarItem(new PedidoItem(item.ProdutoId, item.Quantidade, produto.Preco));
                }
            }

            await pedidoRepository.InsertAsync(pedido, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pedido = await ObterPedidoAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                return false;
            }

            if (!pedido.EfetuarCheckout())
            {
                Notificar("Não foi possível realizar o checkout do pedido.");
            }

            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> AlterarStatusAsync(Guid pedidoId, PedidoStatus pedidoStatus, CancellationToken cancellationToken)
        {
            var pedido = await ObterPedidoAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                return false;
            }

            if (!pedido.AlterarStatus(pedidoStatus))
            {
                Notificar("Não foi possível alterar o status do pedido.");
                return false;
            }

            await pedidoRepository.UpdateAsync(pedido, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public Task<IEnumerable<Pedido>> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            pedidoRepository.ObterTodosPedidosAsync();

        private async Task<Pedido?> ObterPedidoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pedido = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (pedido is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
            }

            return pedido;
        }
    }
}
