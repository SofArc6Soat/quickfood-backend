using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests;

public class PagamentosApiControllerSmokeTest(SmokeTestStartup factory) : IClassFixture<SmokeTestStartup>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task<Guid> CriarPedido()
    {
        var pedidoId = Guid.NewGuid();
        var produtoId = Guid.NewGuid(); 

        var pedido = new PedidoRequestDto
        {
            PedidoId = pedidoId,
            ClienteId = Guid.NewGuid(), 
            Items =
            [
                new PedidoListaItensDto
                {
                    ProdutoId = produtoId,
                    Quantidade = 1
                }
            ]
        };

        var content = new StringContent(JsonConvert.SerializeObject(pedido), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/pedidos", content);
        response.EnsureSuccessStatusCode();

        return pedidoId;
    }
/*
    [Fact]
    public async Task Get_ObterPagamentoPorPedido_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = await CriarPedido(); 

        // Act
        var response = await _client.GetAsync($"/pagamentos/{pedidoId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_Checkout_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = await CriarPedido(); 

        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json"); 

        // Act
        var response = await _client.PostAsync($"/pagamentos/checkout/{pedidoId}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_Notificacoes_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = await CriarPedido(); 

        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json"); 

        // Act
        var response = await _client.PostAsync($"/pagamentos/notificacoes/{pedidoId}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }*/
}