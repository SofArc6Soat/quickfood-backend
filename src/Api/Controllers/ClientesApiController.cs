using Controllers;
using Controllers.Dtos.Request;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("clientes")]
    public class ClientesApiController(IClientesController clientesController, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosClientes(CancellationToken cancellationToken)
        {
            var result = await clientesController.ObterTodosClientesAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf([FromQuery] IdentifiqueSeDto request, CancellationToken cancellationToken)
        {
            var result = await clientesController.IdentificarClienteCpfAsync(request.Cpf, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(ClienteDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await clientesController.CadastrarClienteAsync(request, cancellationToken);

            return CustomResponsePost($"clientes/{request.Id}", request, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCliente([FromRoute] Guid id, ClienteDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != request.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await clientesController.AtualizarClienteAsync(request, cancellationToken);

            return CustomResponsePutPatch(request, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarCliente([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await clientesController.DeletarClienteAsync(id, cancellationToken);

            return CustomResponseDelete(id, result);
        }
    }
}
