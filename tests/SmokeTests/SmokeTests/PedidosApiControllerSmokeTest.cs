using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests;

public class PedidosApiControllerSmokeTest(SmokeTestStartup factory) : IClassFixture<SmokeTestStartup>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly Guid _produtoId = Guid.NewGuid();
    private readonly Guid _pedidoId = Guid.NewGuid();

    [Fact]
    public async Task Post_PedidosEndpoint_CreatesPedido()
    {
        // Arrange: Criação do produto
        await Post_ProdutosEndpoint_CreatesProduto();

        // Criação do pedido
        var pedido = new PedidoRequestDto
        {
            PedidoId = Guid.NewGuid(),
            ClienteId = Guid.NewGuid(), // Opcional
            Items =
        [
            new PedidoListaItensDto
            {
                ProdutoId = _produtoId,
                Quantidade = 2
            }
        ]
        };

        var pedidoContent = CreateContent(pedido);
        var postResponse = await _client.PostAsync("/pedidos", pedidoContent);

        if (!postResponse.IsSuccessStatusCode)
        {
            var postErrorContent = await postResponse.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar o pedido: {postResponse.StatusCode}, Detalhes: {postErrorContent}");
        }

        // Assert
        postResponse.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", postResponse.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Patch_PedidosStatusEndpoint_UpdatesStatus()
    {
        // Arrange: Criação do produto
        await Post_ProdutosEndpoint_CreatesProduto();

        // Criação do pedido
        var pedido = new PedidoRequestDto
        {
            PedidoId = _pedidoId,
            ClienteId = Guid.NewGuid(),
            Items =
        [
            new PedidoListaItensDto
            {
                ProdutoId = _produtoId,
                Quantidade = 2
            }
        ]
        };

        var pedidoContent = CreateContent(pedido);
        var postResponse = await _client.PostAsync("/pedidos", pedidoContent);

        if (!postResponse.IsSuccessStatusCode)
        {
            var postErrorContent = await postResponse.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar o pedido: {postResponse.StatusCode}, Detalhes: {postErrorContent}");
        }

        var statusDto = CreatePedidoStatusRequestDto();
        var content = CreateContent(statusDto);

        // Act
        var response = await _client.PatchAsync($"/pedidos/status/{_pedidoId}", content);

        if (!response.IsSuccessStatusCode)
        {
            var postErrorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar o pedido: {response.StatusCode}, Detalhes: {postErrorContent}");
        }

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    private async Task Post_ProdutosEndpoint_CreatesProduto()
    {
        var produtoDto = new ProdutoRequestDto
        {
            Id = _produtoId,
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10,
            Categoria = "Lanche",
            Ativo = true
        };

        var produtoContent = CreateContent(produtoDto);

        var produtoResponse = await _client.PostAsync("/produtos", produtoContent);

        if (!produtoResponse.IsSuccessStatusCode)
        {
            var produtoErrorContent = await produtoResponse.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar o produto: {produtoResponse.StatusCode}, Detalhes: {produtoErrorContent}");
        }

        // Assert
        produtoResponse.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", produtoResponse.Content.Headers.ContentType.ToString());
    }

    private static PedidoStatusRequestDto CreatePedidoStatusRequestDto() => new()
    {
        Status = "EmPreparacao"
    };

    private StringContent CreateContent(object dto) => new(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
}