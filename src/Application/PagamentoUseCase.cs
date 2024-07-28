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
            var pedidoExistenteDto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (pedidoExistenteDto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            _ = Enum.TryParse(pedidoExistenteDto.Status, out PedidoStatus status);
            var pedido = new Pedido(pedidoExistenteDto.Id, pedidoExistenteDto.NumeroPedido, pedidoExistenteDto.ClienteId, status, pedidoExistenteDto.ValorTotal, pedidoExistenteDto.DataPedido);

            if (!pedido.EfetuarCheckout())
            {
                Notificar("Não foi possível realizar o checkout do pedido.");
                return false;
            }

            var pagamentoDtoExistente = pagamentoRepository.Find(e => e.PedidoId == pedido.Id).Any();

            if (pagamentoDtoExistente)
            {
                Notificar("Pagamento já existente para o pedido, aguarde a confirmação do seu Pix.");
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

            var pagamento = new Pagamento(pedidoId, pedido.ValorTotal);
            pagamento.GerarQrCodePix();
            pagamento.AlterarStatusPagamentoParaPendente();

            var pagementoDto = new PagamentoDto
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status.ToString(),
                QrCodePix = pagamento.QrCodePix,
                Valor = pedido.ValorTotal,
                DataPagamento = pagamento.DataPagamento
            };

            await pagamentoRepository.InsertAsync(pagementoDto, cancellationToken);

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<bool> NotificarPagamentoAsync(Guid pedidoId, CancellationToken cancellationToken)
        {
            var pagamentoExistenteDto = pagamentoRepository.Find(e => e.PedidoId == pedidoId).FirstOrDefault();

            if (pagamentoExistenteDto is null)
            {
                Notificar($"Pagamento inexistente para o {pedidoId}, verifique se o pedido foi efetuado corretamente");
                return false;
            }

            _ = Enum.TryParse(pagamentoExistenteDto.Status, out StatusPagamento status);
            var pagamento = new Pagamento(pagamentoExistenteDto.Id, pagamentoExistenteDto.PedidoId, status, pagamentoExistenteDto.Valor, pagamentoExistenteDto.QrCodePix, pagamentoExistenteDto.DataPagamento);
            pagamento.AlterarStatusPagamentoParaPago();

            var pagementoDto = new PagamentoDto
            {
                Id = pagamento.Id,
                PedidoId = pagamento.PedidoId,
                Status = pagamento.Status.ToString(),
                QrCodePix = pagamento.QrCodePix,
                Valor = pagamento.Valor,
                DataPagamento = pagamento.DataPagamento
            };

            await pagamentoRepository.UpdateAsync(pagementoDto, cancellationToken);

            var pedidoExistenteDto = await pedidoRepository.FindByIdAsync(pedidoId, cancellationToken);

            if (pedidoExistenteDto is null)
            {
                Notificar($"Pedido {pedidoId} não encontrado.");
                return false;
            }

            _ = Enum.TryParse(pedidoExistenteDto.Status, out PedidoStatus pedidoStatus);

            var pedido = new Pedido(pedidoExistenteDto.Id, pedidoExistenteDto.NumeroPedido, pedidoExistenteDto.ClienteId, pedidoStatus, pedidoExistenteDto.ValorTotal, pedidoExistenteDto.DataPedido);
            pedido.AlterarStatusParaRecebibo();

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

            return await pagamentoRepository.UnitOfWork.CommitAsync(cancellationToken);
        }

        public async Task<string> ObterPagamentoPorPedidoAsync(Guid pedidoId, CancellationToken cancellationToken) =>
            await pagamentoRepository.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);
    }
}
