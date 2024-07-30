﻿using Domain.Entities;

namespace Gateways
{
    public interface IPagamentoGateway
    {
        Pagamento? ObterPagamentoPorPedido(Guid pedidoId, CancellationToken cancellationToken);
        Task<bool> CadastrarPagamentoAsync(Pagamento pagamento, CancellationToken cancellationToken);
    }
}
