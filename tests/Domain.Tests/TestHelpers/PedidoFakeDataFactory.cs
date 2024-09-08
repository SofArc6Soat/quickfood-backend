using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Tests.TestHelpers;

public static class PedidoFakeDataFactory
{
    public static Pedido CriarPedidoValido()
    {
        return new Pedido(Guid.NewGuid(), 1, Guid.NewGuid(), PedidoStatus.Rascunho, 100.00m, DateTime.Now);
    }

    public static Pedido CriarPedidoInvalido()
    {
        return new Pedido(Guid.NewGuid(), 0, null, PedidoStatus.Rascunho, -100.00m, DateTime.MinValue);
    }

    public static PedidoItem CriarPedidoItemValido()
    {
        return new PedidoItem(Guid.NewGuid(), 2, 10.00m);
    }
}