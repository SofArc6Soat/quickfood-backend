using Domain.Tests.TestHelpers;
using Gateways;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Domain.Tests.Gateway
{
    public class ClienteGatewayTests
    {
        private readonly Mock<IClienteRepository> _mockClienteRepository;
        private readonly ClienteGateway _clienteGateway;

        public ClienteGatewayTests()
        {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _clienteGateway = new ClienteGateway(_mockClienteRepository.Object);
        }

        [Fact]
        public async Task DeveCadastrarClienteAsync()
        {
            // Arrange
            var cliente = ClienteFakeDataFactory.CriarClienteValido();
            _mockClienteRepository.Setup(repo => repo.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);
            _mockClienteRepository.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);

            // Act
            var result = await _clienteGateway.CadastrarClienteAsync(cliente, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockClienteRepository.Verify(repo => repo.InsertAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockClienteRepository.Verify(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeveAtualizarClienteAsync()
        {
            // Arrange
            var cliente = ClienteFakeDataFactory.CriarClienteValido();
            _mockClienteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);
            _mockClienteRepository.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);

            // Act
            var result = await _clienteGateway.AtualizarClienteAsync(cliente, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockClienteRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ClienteDb>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockClienteRepository.Verify(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeveDeletarClienteAsync()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            _mockClienteRepository.Setup(repo => repo.DeleteAsync(clienteId, It.IsAny<CancellationToken>()))
                                  .Returns(Task.CompletedTask);
            _mockClienteRepository.Setup(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);

            // Act
            var result = await _clienteGateway.DeletarClienteAsync(clienteId, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockClienteRepository.Verify(repo => repo.DeleteAsync(clienteId, It.IsAny<CancellationToken>()), Times.Once);
            _mockClienteRepository.Verify(repo => repo.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void DeveVerificarClienteExistente()
        {
            // Arrange
            var cliente = ClienteFakeDataFactory.CriarClienteValido();
            var clienteDb = ClienteFakeDataFactory.CriarClienteDbValido();

            _mockClienteRepository.Setup(repo => repo.Find(
                It.IsAny<Expression<Func<ClienteDb, bool>>>(),
                It.IsAny<CancellationToken>()
            )).Returns(new List<ClienteDb> { clienteDb }.AsQueryable());

            // Act
            var result = _clienteGateway.VerificarClienteExistente(cliente.Id, cliente.Cpf, cliente.Email, CancellationToken.None);

            Console.WriteLine($"Resultado do teste: {result}");

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task DeveObterTodosClientesAsync()
        {
            // Arrange
            var clientesDb = new List<ClienteDb>
                {
                    new() { Id = Guid.NewGuid(), Nome = "João Silva", Email = "joao@teste.com", Cpf = "12345678901", Ativo = true }
                };
            _mockClienteRepository.Setup(repo => repo.ObterTodosClientesAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(clientesDb);

            // Act
            var result = await _clienteGateway.ObterTodosClientesAsync(CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal(clientesDb.First().Nome, result.First().Nome);
        }

        [Fact]
        public async Task DeveIdentificarClienteCpfAsync()
        {
            // Arrange
            var clienteDb = new ClienteDb { Id = Guid.NewGuid(), Nome = "João Silva", Email = "joao@teste.com", Cpf = "12345678901", Ativo = true };
            _mockClienteRepository.Setup(repo => repo.IdentificarClienteCpfAsync("12345678901", It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(clienteDb);

            // Act
            var result = await _clienteGateway.IdentificarClienteCpfAsync("12345678901", CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(clienteDb.Nome, result?.Nome);
        }
    }
}