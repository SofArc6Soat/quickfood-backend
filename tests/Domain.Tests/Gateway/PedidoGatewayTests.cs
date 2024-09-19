using Domain.ValueObjects;
using FluentAssertions;
using Gateways;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Domain.Tests.Gateway;

public class PedidoGatewayTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly PedidoGateway _pedidoGateway;

    public PedidoGatewayTests()
    {
        _pedidoRepositoryMock = new Mock<IPedidoRepository>();
        _pedidoGateway = new PedidoGateway(_pedidoRepositoryMock.Object);
    }

   /* [Fact]
    public async Task VerificarPedidoExistenteAsync_DeveRetornarTrue_QuandoPedidoExistir()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pedidoRepositoryMock.Setup(repo => repo.Find(It.IsAny<Expression<Func<PedidoDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<PedidoDb> { new PedidoDb { Id = pedidoId } }.AsQueryable());

        // Act
        var resultado = await _pedidoGateway.VerificarPedidoExistenteAsync(pedidoId, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public async Task ObterPedidoAsync_DeveRetornarPedido_QuandoPedidoExistir()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedidoDb = new PedidoDb
        {
            Id = pedidoId,
            NumeroPedido = 1,
            ClienteId = Guid.NewGuid(),
            Status = PedidoStatus.Rascunho.ToString(),
            ValorTotal = 100.00m,
            DataPedido = DateTime.Now
        };

        // Configurar o mock para retornar um Task que resolve para IEnumerable<PedidoDb>
        _pedidoRepositoryMock
            .Setup(repo => repo.Find(It.IsAny<Expression<Func<PedidoDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((IEnumerable<PedidoDb>)Task.FromResult(new List<PedidoDb> { pedidoDb }.AsEnumerable()));

        // Act
        var resultado = await _pedidoGateway.ObterPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(pedidoId);
        resultado.NumeroPedido.Should().Be(pedidoDb.NumeroPedido);
        resultado.ClienteId.Should().Be(pedidoDb.ClienteId);
        resultado.Status.Should().Be(Enum.Parse<PedidoStatus>(pedidoDb.Status));
        resultado.ValorTotal.Should().Be(pedidoDb.ValorTotal);
        resultado.DataPedido.Should().Be(pedidoDb.DataPedido);
    }*/
}