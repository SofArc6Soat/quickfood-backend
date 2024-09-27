using Domain.Entities;
using Gateways.Dtos.Response;

namespace Gateways
{
    public interface ICognitoGateway
    {
        Task<bool> CriarUsuarioClienteAsync(Cliente cliente, string senha, CancellationToken cancellationToken);
        Task<bool> CriarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
        Task<TokenUsuario?> IdentifiqueSe(string? email, string? cpf, string senha, CancellationToken cancellationToken);
        Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken);
        Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenha solicitarRecuperacaoSenha, CancellationToken cancellationToken);
        Task<bool> EfetuarResetSenhaAsync(ResetarSenha resetarSenha, CancellationToken cancellationToken);
    }
}
