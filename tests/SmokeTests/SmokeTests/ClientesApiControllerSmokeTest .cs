using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests
{
    public class ClientesApiControllerSmokeTest(SmokeTestStartup factory) : IClassFixture<SmokeTestStartup>
    {
        private readonly HttpClient _client = factory.CreateClient();
        private readonly string cpf = "39039023069";

        [Fact]
        public async Task Get_ClientesEndpoint_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/clientes");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        // Nao podemos executar os testes abaixo, pois estão criando um usuário no Cognito

        //[Fact]
        //public async Task Get_IdentifiqueSeEndpoint_ReturnsSuccess()
        //{
        //    // Arrange
        //    var cliente = CreateClienteRequestDto();

        //    // Act
        //    await _client.PostAsync("/clientes", CreateContent(cliente));
        //    var response = await _client.GetAsync($"/clientes/identifique-se?cpf={cpf}");

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        //}

        //[Fact]
        //public async Task Post_ClientesEndpoint_CreatesCliente()
        //{
        //    // Arrange
        //    var cliente = CreateClienteRequestDto();

        //    // Act
        //    var response = await _client.PostAsync("/clientes", CreateContent(cliente));

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        //}

        //[Fact]
        //public async Task Put_ClientesEndpoint_UpdatesCliente()
        //{
        //    // Arrange
        //    var clienteId = Guid.NewGuid();
        //    var cliente = CreateClienteRequestDto(clienteId);

        //    await _client.PostAsync("/clientes", CreateContent(cliente));

        //    // Act
        //    var response = await _client.PutAsync($"/clientes/{clienteId}", CreateContent(cliente));

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        //}

        //[Fact]
        //public async Task Delete_ClientesEndpoint_DeletesCliente()
        //{
        //    // Arrange
        //    var clienteId = Guid.NewGuid();
        //    var cliente = CreateClienteRequestDto(clienteId);

        //    await _client.PostAsync("/clientes", CreateContent(cliente));

        //    // Act
        //    var response = await _client.DeleteAsync($"/clientes/{clienteId}");

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //}

        private ClienteRequestDto CreateClienteRequestDto(Guid? id = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Nome = "Cliente Teste",
            Email = "cliente@test.com",
            Cpf = cpf,
            Senha = "Teste@123",
            Ativo = true
        };

        private static StringContent CreateContent(ClienteRequestDto cliente) => new(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");
    }
}