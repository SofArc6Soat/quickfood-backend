using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [AllowAnonymous]
    [Route("usuarios")]
    public class UsuariosApiController(IUsuariosController usuarioController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost("admin")]
        public async Task<IActionResult> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

            usuarioRequestDto.Senha = "*******";

            return CustomResponsePost($"usuarios/{usuarioRequestDto.Id}", usuarioRequestDto, result);
        }

        [HttpPost("email-verificacao:confirmar")]
        public async Task<IActionResult> ConfirmarEmailVerificaoAsync([FromBody] ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.ConfirmarEmailVerificacaoAsync(confirmarEmailVerificacaoDto, cancellationToken);

            return CustomResponsePost($"usuarios/email-verificacao:confirmar", confirmarEmailVerificacaoDto, result);
        }

        [HttpPost("esquecia-senha:solicitar")]
        public async Task<IActionResult> SolicitarRecuperacaoSenhaAsync([FromBody] SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenhaDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenhaDto, cancellationToken);

            return CustomResponsePost($"usuarios/email-verificacao:solicitar", solicitarRecuperacaoSenhaDto, result);
        }

        [HttpPost("esquecia-senha:resetar")]
        public async Task<IActionResult> EfetuarResetSenhaAsync([FromBody] ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.EfetuarResetSenhaAsync(resetarSenhaDto, cancellationToken);

            resetarSenhaDto.NovaSenha = "*******";

            return CustomResponsePost($"usuarios/email-verificacao:resetar", resetarSenhaDto, result);
        }

        [HttpPost("admin/identifique-se")]
        public async Task<IActionResult> IdentificarClienteCpf(IdentifiqueSeAdminRequestDto request, CancellationToken cancellationToken)
        {
            var result = await usuarioController.IdentificarAdminAsync(request.Email, request.Senha, cancellationToken);

            request.Senha = "*******";

            return result == null
                ? CustomResponsePost($"admin/identifique-se", request, false)
                : CustomResponsePost($"admin/identifique-se", result, true);
        }
    }
}
