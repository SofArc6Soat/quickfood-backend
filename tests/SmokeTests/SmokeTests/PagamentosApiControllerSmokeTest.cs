using Gateways.Dtos.Request;
using Newtonsoft.Json;
using System.Text;

namespace SmokeTests.SmokeTests
{
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
    }
}