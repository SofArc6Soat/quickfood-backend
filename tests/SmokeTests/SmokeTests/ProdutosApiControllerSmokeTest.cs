using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests;

public class ProdutosApiControllerSmokeTest(SmokeTestStartup factory) : IClassFixture<SmokeTestStartup>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Get_ProdutosEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/produtos");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Get_ProdutosCategoriaEndpoint_ReturnsSuccess()
    {
        // Act
        var response = await _client.GetAsync("/produtos/categoria?categoria=Lanche");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_ProdutosEndpoint_CreatesProduto()
    {
        // Arrange
        var produto = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto teste",
            Descricao = "Um produto novo",
            Preco = 10,
            Categoria = "Lanche",
            Ativo = true
        };
        var content = new StringContent(JsonConvert.SerializeObject(produto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/produtos", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Put_ProdutosEndpoint_UpdatesProduto()
    {
        // Arrange
        // Crie um produto inicial
        var produtoInicial = new ProdutoRequestDto
        {
            Id = Guid.NewGuid(), // Gera um novo ID
            Nome = "Produto Inicial",
            Descricao = "Descrição Inicial",
            Preco = 10,
            Categoria = "Lanche",
            Ativo = true
        };

        // Crie o produto usando um método auxiliar ou uma chamada HTTP
        var initialContent = new StringContent(
            JsonConvert.SerializeObject(produtoInicial),
            Encoding.UTF8,
            "application/json"
        );
        var postResponse = await _client.PostAsync("/produtos", initialContent);
        postResponse.EnsureSuccessStatusCode(); // Certifique-se de que o produto foi criado

        // Atualize o produto
        var produtoAtualizado = new ProdutoRequestDto
        {
            Id = produtoInicial.Id, // Use o mesmo ID
            Nome = "Produto Atualizado",
            Descricao = "Descrição Atualizada",
            Preco = 20,
            Categoria = "Lanche",
            Ativo = true
        };

        var updateContent = new StringContent(
            JsonConvert.SerializeObject(produtoAtualizado),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PutAsync($"/produtos/{produtoAtualizado.Id}", updateContent);

        // Assert
        response.EnsureSuccessStatusCode(); // Verifica se a atualização foi bem-sucedida
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Delete_ProdutosEndpoint_DeletesProduto()
    {
        // Arrange
        var produtoId = Guid.NewGuid();

        // Primeiro, crie o produto para garantir que ele existe antes de deletar
        var produtoParaCriar = new ProdutoRequestDto
        {
            Id = produtoId,
            Nome = "Produto para Deletar",
            Descricao = "Descrição do produto para deletar",
            Preco = 15,
            Categoria = "Lanche",
            Ativo = true
        };

        var createContent = new StringContent(JsonConvert.SerializeObject(produtoParaCriar), Encoding.UTF8, "application/json");
        var createResponse = await _client.PostAsync("/produtos", createContent);
        createResponse.EnsureSuccessStatusCode();

        // Agora, delete o produto
        var response = await _client.DeleteAsync($"/produtos/{produtoId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}