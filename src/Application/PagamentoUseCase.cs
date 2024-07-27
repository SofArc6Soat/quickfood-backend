using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Domain.ValueObjects;
using Infra.Dto;
using Infra.Repositories;

namespace UseCases
{
    public class PagamentoUseCase(IPedidoRepository pedidoRepository, IPagamentoRepository pagamentoRepository, INotificador notificador) : BaseUseCase(notificador), IPagamentoUseCase
    {
        public async Task<bool> EfetuarCheckoutAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var dto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (dto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            _ = Enum.TryParse(dto.Status, out PedidoStatus status);
            var pedido = new Pedido(dto.Id, dto.NumeroPedido, dto.ClienteId, status, dto.ValorTotal, dto.DataCriacao);

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

            var pedidoItensDto = new List<PedidoItemDto>();
            foreach (var item in pedido.PedidoItems)
            {
                pedidoItensDto.Add(new PedidoItemDto(item.Id, pedido.Id, item.ProdutoId, item.Quantidade, item.ValorUnitario));
            }

            var peditoDto = new PedidoDto(pedido.Id, pedido.NumeroPedido, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.DataCriacao, pedidoItensDto);
            await pedidoRepository.UpdateAsync(peditoDto, cancellationToken);

            var pagamento = new Pagamento(pedidoId, pedido.ValorTotal);
            pagamento.GerarQrCodePix();
            pagamento.AlterarStatusPagamentoParaPendente();

            var pagementoDto = new PagamentoDto(pagamento.Id, pagamento.PedidoId, pagamento.Status.ToString(), pagamento.Valor, pagamento.QrCodePix, pagamento.DataCriacao);
            await pagamentoRepository.InsertAsync(pagementoDto, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> NotificarPagamentoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var dto = pagamentoRepository.Find(e => e.PedidoId == pedidoId).FirstOrDefault();

            if (dto is null)
            {
                Notificar($"Pagamento inexistente para o {pedidoId}, verifique se o pedido foi efetuado corretamente");
                return false;
            }

            _ = Enum.TryParse(dto.Status, out StatusPagamento status);
            var pagamento = new Pagamento(dto.Id, dto.PedidoId, status, dto.Valor, dto.QrCodePix, dto.DataCriacao);
            pagamento.AlterarStatusPagamentoParaPago();

            var pagementoDto = new PagamentoDto(pagamento.Id, pagamento.PedidoId, pagamento.Status.ToString(), pagamento.Valor, pagamento.QrCodePix, pagamento.DataCriacao);
            await pagamentoRepository.UpdateAsync(pagementoDto, cancellationToken);

            var pedidoDto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (pedidoDto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            _ = Enum.TryParse(dto.Status, out PedidoStatus pedidoStatus);
            var pedido = new Pedido(pedidoDto.Id, pedidoDto.NumeroPedido, pedidoDto.ClienteId, pedidoStatus, pedidoDto.ValorTotal, pedidoDto.DataCriacao);
            pedido.AlterarStatusParaRecebibo();

            var pedidoItensDto = new List<PedidoItemDto>();
            foreach (var item in pedido.PedidoItems)
            {
                pedidoItensDto.Add(new PedidoItemDto(item.Id, pedido.Id, item.ProdutoId, item.Quantidade, item.ValorUnitario));
            }

            var pedidoNovoDto = new PedidoDto(pedido.Id, pedido.NumeroPedido, pedido.ClienteId, pedido.Status.ToString(), pedido.ValorTotal, pedido.DataCriacao, pedidoItensDto);
            await pedidoRepository.UpdateAsync(pedidoNovoDto, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken) =>
            await pagamentoRepository.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
    }
}
