using Controllers;
using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Gateways.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("usuarios")]
    public class UsuariosApiController(IUsuariosController usuarioController, INotificador notificador) : MainController(notificador)
    {
        [HttpPost("admin")]
        public async Task<IActionResult> CadastrarUsuario(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await usuarioController.CadastrarUsuarioAsync(usuarioRequestDto, cancellationToken);

            usuarioRequestDto.Senha = "*******";

            return CustomResponsePost($"usuarios/{usuarioRequestDto.Id}", usuarioRequestDto, result);
        }
    }
}
