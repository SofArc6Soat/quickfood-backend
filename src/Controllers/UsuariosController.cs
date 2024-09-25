using Domain.Entities;
using Gateways.Dtos.Request;
using UseCases;

namespace Controllers
{
    public class UsuariosController(IUsuarioUseCase usuarioUseCase) : IUsuariosController
    {
        public async Task<bool> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken)
        {
            var usuario = new Usuario(usuarioRequestDto.Id, usuarioRequestDto.Nome, usuarioRequestDto.Email, usuarioRequestDto.Ativo);

            return await usuarioUseCase.CadastrarUsuarioAsync(usuario, usuarioRequestDto.Senha, cancellationToken);
        }
    }
}
