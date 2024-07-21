using Application.Models.Request;
using Application.UseCases;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("pedidos")]
    public class PedidosController(IPedidoUseCase pedidoUseCase, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosPedidos(CancellationToken cancellationToken)
        {
            var result = await pedidoUseCase.ObterTodosPedidosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPedido(PedidoRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoUseCase.CadastrarPedidoAsync(request, cancellationToken);

            return CustomResponsePost($"pedidos/{request.PedidoId}", request, result);
        }

        [HttpPatch("checkout/{pedidoId:guid}")]
        public async Task<IActionResult> Checkout([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoUseCase.EfetuarCheckoutAsync(pedidoId, cancellationToken);

            return CustomResponsePutPatch(pedidoId, result);
        }

        [HttpPatch("status/{pedidoId:guid}")]
        public async Task<IActionResult> AlterarStatus([FromRoute] Guid pedidoId, [FromBody] PedidoStatusRequest pedidoStatus, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pedidoUseCase.AlterarStatusAsync(pedidoId, pedidoStatus.Status, cancellationToken);

            return CustomResponsePutPatch(pedidoStatus, result);
        }
    }
}
