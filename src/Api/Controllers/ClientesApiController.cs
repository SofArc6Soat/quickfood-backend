using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("clientes")]
    public class ClientesApiController(IClientesController clientesController, INotificador notificador) : MainController(notificador)
    {
        [Authorize(Policy = "AdminRole")]
        [HttpGet]
        public async Task<IActionResult> ObterTodosClientes(CancellationToken cancellationToken)
        {
            var result = await clientesController.ObterTodosClientesAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [AllowAnonymous]
        [HttpPost("identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf(IdentifiqueSeRequestDto request, CancellationToken cancellationToken)
        {
            var result = await clientesController.IdentificarClienteCpfAsync(request.Cpf, request.Senha, cancellationToken);

            request.Senha = "*******";

            return result == null
                ? CustomResponsePost($"clientes/identifique-se", request, false)
                : CustomResponsePost($"clientes/identifique-se", result, true);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CadastrarCliente(ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await clientesController.CadastrarClienteAsync(clienteRequestDto, cancellationToken);

            clienteRequestDto.Senha = "*******";

            return CustomResponsePost($"clientes/{clienteRequestDto.Id}", clienteRequestDto, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarCliente([FromRoute] Guid id, ClienteRequestDto clienteRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != clienteRequestDto.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await clientesController.AtualizarClienteAsync(clienteRequestDto, cancellationToken);

            return CustomResponsePutPatch(clienteRequestDto, result);
        }

        [Authorize(Policy = "AdminRole")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarCliente([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await clientesController.DeletarClienteAsync(id, cancellationToken);

            return CustomResponseDelete(id, result);
        }
    }
}
