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

            var pedidoItensDto = new List<PedidoItemDto>();
            foreach (var item in pedido.PedidoItems)
            {
                pedidoItensDto.Add(new PedidoItemDto(item.Id, pedido.Id, item.ProdutoId, item.Quantidade, item.ValorUnitario));
            }

            var pedidoDto = new PedidoDto(pedido.Id, pedido.NumeroPedido, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.DataCriacao, pedidoItensDto);

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

            var pedidoItensDto = new List<PedidoItemDto>();
            foreach (var item in pedido.PedidoItems)
            {
                pedidoItensDto.Add(new PedidoItemDto(item.Id, pedido.Id, item.ProdutoId, item.Quantidade, item.ValorUnitario));
            }

            var pedidoDto = new PedidoDto(pedido.Id, pedido.NumeroPedido, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.DataCriacao, pedidoItensDto);
            await pedidoRepository.UpdateAsync(pedidoDto, cancellationToken);

            return await pedidoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<IEnumerable<Pedido>> ObterTodosPedidosAsync(CancellationToken cancellationToken)
        {
            var dto = await pedidoRepository.ObterTodosPedidosAsync();

            if (dto.Any())
            {
                var pedido = new List<Pedido>();
                foreach (var item in dto)
                {
                    _ = Enum.TryParse(item.Status, out PedidoStatus status);
                    pedido.Add(new Pedido(item.Id, item.NumeroPedido, item.ClienteId, status, item.ValorTotal, item.DataCriacao));
                }

                return pedido;
            }

            return [];
        }

        private async Task<Pedido?> ObterPedidoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var dto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (dto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return null;
            }

            _ = Enum.TryParse(dto.Status, out PedidoStatus status);
            var pedido = new Pedido(dto.Id, dto.NumeroPedido, dto.ClienteId, status, dto.ValorTotal, dto.DataCriacao);

            return pedido;
        }

        public async Task<string> ObterTodosPedidosOrdenadosAsync(CancellationToken cancellationToken) =>
            await pedidoRepository.ObterTodosPedidosOrdenadosAsync(cancellationToken);
    }
}
