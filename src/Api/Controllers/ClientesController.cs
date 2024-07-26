using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Microsoft.AspNetCore.Mvc;
using UseCases.Models.Request;
using UseCases.UseCases;

namespace Api.Controllers
{
    [Route("clientes")]
    public class ClientesController(IClienteUseCase clienteUseCase, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosClientes(CancellationToken cancellationToken)
        {
            var result = await clienteUseCase.ObterTodosClientesAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf([FromQuery] IdentifiqueSeRequest request, CancellationToken cancellationToken)
        {
            var result = await clienteUseCase.IdentificarClienteCpfAsync(request, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(ClienteRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await clienteUseCase.CadastrarClienteAsync(request, cancellationToken);

            return CustomResponsePost($"clientes/{request.Id}", request, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCliente([FromRoute] Guid id, ClienteRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != request.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await clienteUseCase.AtualizarClienteAsync(request, cancellationToken);

            return CustomResponsePutPatch(request, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarCliente([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await clienteUseCase.DeletarClienteAsync(id, cancellationToken);

            return CustomResponseDelete(id, result);
        }
    }
}
