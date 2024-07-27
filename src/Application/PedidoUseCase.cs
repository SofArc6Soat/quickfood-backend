using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;

namespace UseCases
{
    public class PedidoUseCase(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IPedidoUseCase
    {
        public async Task<bool> CadastrarPedidoAsync(Pedido pedido, List<PedidoListaItens> itens, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(pedido);

            var pedidoExistente = await pedidoRepository.FindByIdAsync(pedido.Id, cancellationToken);

            if (pedidoExistente is not null)
            {
                Notificar("Pedido já existente");
                return false;
            }

            if (!ExecutarValidacao(new ValidarPedido(), pedido))
            {
                return false;
            }

            foreach (var item in itens)
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

            if (pedido.PedidoItems.Count == 0)
            {
                Notificar("O pedido precisa ter pelo menos um item.");
                return false;
            }

            await pedidoRepository.InsertAsync(pedido, cancellationToken);

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

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosOrdenadosAsync(cancellationToken);
    }
}
