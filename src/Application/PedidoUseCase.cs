using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

namespace UseCases
{
    public class PedidoUseCase(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository, INotificador notificador) : BaseUseCase(notificador), IPedidoUseCase
    {
        public async Task<bool> CadastrarPedidoAsync(Pedido pedido, List<PedidoListaItens> itens, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(pedido);

            var pedidoDtoExistente = await pedidoRepository.FindByIdAsync(pedido.Id, cancellationToken);

            if (pedidoDtoExistente is not null)
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
                var produtoDto = await produtoRepository.FindByIdAsync(item.ProdutoId, cancellationToken);

                if (produtoDto is null)
                {
                    Notificar($"Produto {item.ProdutoId} não encontrado.");
                }
                else
                {
                    pedido.AdicionarItem(new PedidoItem(item.ProdutoId, item.Quantidade, produtoDto.Preco));
                }
            }

            if (pedido.PedidoItems.Count == 0)
            {
                Notificar("O pedido precisa ter pelo menos um item.");
                return false;
            }

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ClienteId = pedido.ClienteId,
                Status = pedido.Status.ToString(),
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido
            };

            foreach (var item in pedido.PedidoItems)
            {
                pedidoDto.Itens.Add(new PedidoItemDto
                {
                    PedidoId = pedidoDto.Id,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario
                });
            }

            await pedidoRepository.InsertAsync(pedidoDto, cancellationToken);

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

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                ClienteId = pedido.ClienteId,
                Status = pedido.Status.ToString(),
                ValorTotal = pedido.ValorTotal,
                DataPedido = pedido.DataPedido
            };

            await pedidoRepository.UpdateAsync(pedidoDto, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<string> ObterTodosPedidosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosAsync(cancellationToken);

        private async Task<Pedido?> ObterPedidoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var dto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (dto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return null;
            }

            _ = Enum.TryParse(dto.Status, out PedidoStatus status);
            var pedido = new Pedido(dto.Id, dto.NumeroPedido, dto.ClienteId, status, dto.ValorTotal, dto.DataPedido);

            return pedido;
        }

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosOrdenadosAsync(cancellationToken);
    }
}
