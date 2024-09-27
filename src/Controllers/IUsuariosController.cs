using Gateways.Dtos.Request;
using Gateways.Dtos.Response;

namespace Controllers
{
    public interface IUsuariosController
    {
        Task<bool> CadastrarUsuarioAsync(UsuarioRequestDto usuarioRequestDto, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentificarAdminAsync(string email, string senha, CancellationToken cancellationToken);
    }
}
