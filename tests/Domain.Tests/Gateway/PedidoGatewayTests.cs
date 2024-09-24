using Gateways;
using Infra.Repositories;
using Moq;

namespace Domain.Tests.Gateway
{
    public class PedidoGatewayTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly PedidoGateway _pedidoGateway;

        public PedidoGatewayTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _pedidoGateway = new PedidoGateway(_pedidoRepositoryMock.Object);
        }
    }
}