using Controllers;
using Controllers.Dtos.Request;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("pedidos")]
    public class PedidosApiController(IPedidoController pedidoController, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos(CancellationToken cancellationToken)
        {
            var result = await pedidoController.ObterTodosPedidosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("ordenados")]
        public async Task<ContentResult> ObterTodosPedidosOrdenados(CancellationToken cancellationToken)
        {
            var result = await pedidoController.ObterTodosPedidosOrdenadosAsync(cancellationToken);

            return Content(result, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPedido(PedidoDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoController.CadastrarPedidoAsync(request, cancellationToken);

            return CustomResponsePost($"pedidos/{request.PedidoId}", request, result);
        }

        [HttpPatch("status/{pedidoId:guid}")]
        public async Task<IActionResult> AlterarStatus([FromRoute] Guid pedidoId, [FromBody] PedidoStatusDto pedidoStatusDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoController.AlterarStatusAsync(pedidoId, pedidoStatusDto, cancellationToken);

            return CustomResponsePutPatch(pedidoStatusDto, result);
        }
    }
}
