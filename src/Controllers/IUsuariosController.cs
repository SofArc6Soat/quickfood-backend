using Gateways.Dtos.Request;

namespace Controllers
{
    public interface IUsuariosController
    {
        Task<bool> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken);
    }
}
