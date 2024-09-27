using Domain.Entities;
using Gateways.Dtos.Response;

namespace UseCases
{
    public interface IUsuarioUseCase
    {
        Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenha solicitarRecuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetarSenha resetarSenha, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentificarAdminAsync(string email, string senha, CancellationToken cancellationToken);
    }
}