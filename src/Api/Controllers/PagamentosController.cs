using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Mvc;
using UseCases.UseCases;

namespace Api.Controllers
{
    [Route("pagamentos")]
    public class PagamentosController(IPagamentoUseCase pagamentoUseCase, INotificador notificador) : MainController(notificador)
    {
        [HttpGet("{pedidoId:guid}")]
        public async Task<ContentResult> ObterPagamentoPorPedido([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            var result = await pagamentoUseCase.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken);

            return Content(result, "application/json");
        }

        [HttpPost("checkout/{pedidoId:guid}")]
        public async Task<IActionResult> Checkout([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pagamentoUseCase.EfetuarCheckoutAsync(pedidoId, cancellationToken);

            return CustomResponsePutPatch(pedidoId, result);
        }

        [HttpPost("notificacoes/{pedidoId:guid}")]
        public async Task<IActionResult> Notificacoes([FromRoute] Guid pedidoId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await pagamentoUseCase.NotificarPagamentoAsync(pedidoId, cancellationToken);

            return CustomResponsePutPatch(pedidoId, result);
        }
    }
}
