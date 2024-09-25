using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests
{
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
            var produto = CreateProdutoRequestDto();
            var content = CreateContent(produto);

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
            var produtoInicial = CreateProdutoRequestDto();
            await _client.PostAsync("/produtos", CreateContent(produtoInicial));

            var produtoAtualizado = new ProdutoRequestDto
            {
                Id = produtoInicial.Id,
                Nome = "Produto Atualizado",
                Descricao = "Descrição Atualizada",
                Preco = 20,
                Categoria = "Lanche",
                Ativo = true
            };

            var updateContent = CreateContent(produtoAtualizado);

            // Act
            var response = await _client.PutAsync($"/produtos/{produtoAtualizado.Id}", updateContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Delete_ProdutosEndpoint_DeletesProduto()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var produtoParaCriar = CreateProdutoRequestDto(produtoId);
            await _client.PostAsync("/produtos", CreateContent(produtoParaCriar));

            // Act
            var response = await _client.DeleteAsync($"/produtos/{produtoId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private static ProdutoRequestDto CreateProdutoRequestDto(Guid? id = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Nome = "Produto Teste",
            Descricao = "Descrição do Produto Teste",
            Preco = 10,
            Categoria = "Lanche",
            Ativo = true
        };

        private static StringContent CreateContent(ProdutoRequestDto produto) => new(JsonConvert.SerializeObject(produto), Encoding.UTF8, "application/json");
    }
}
