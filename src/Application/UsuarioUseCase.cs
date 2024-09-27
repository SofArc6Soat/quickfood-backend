using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Gateways.Dtos.Response;

namespace UseCases
{
    public class UsuarioUseCase(IUsuarioGateway usuarioGateway, ICognitoGateway cognitoGateway, INotificador notificador) : BaseUseCase(notificador), IUsuarioUseCase
    {
        public async Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (usuarioGateway.VerificarUsuarioExistente(usuario.Id, usuario.Email, cancellationToken))
            {
                Notificar("Usuário já existente");
                return false;
            }

            if (ExecutarValidacao(new ValidarUsuario(), usuario)
                   && await usuarioGateway.CadastrarUsuarioAsync(usuario, senha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro ao cadastrar o usuario com o e-mail: {usuario.Email}");
            return false;
        }

        public async Task<bool> ConfirmarEmailVerificacaoAsync(EmailVerificacao emailVerificacao, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(emailVerificacao);

            if (ExecutarValidacao(new ValidarEmailVerificacao(), emailVerificacao)
                   && await cognitoGateway.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar a verificação do e-mail: {emailVerificacao.Email}");
            return false;
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenha solicitarRecuperacaoSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(solicitarRecuperacaoSenha);

            if (ExecutarValidacao(new ValidarSolicitacaoRecuperacaoSenha(), solicitarRecuperacaoSenha)
                   && await cognitoGateway.SolicitarRecuperacaoSenhaAsync(solicitarRecuperacaoSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro solicitar a recuperacao de senha do e-mail: {solicitarRecuperacaoSenha.Email}");
            return false;
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetarSenha resetarSenha, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(resetarSenha);

            if (ExecutarValidacao(new ValidarResetSenha(), resetarSenha)
                   && await cognitoGateway.EfetuarResetSenhaAsync(resetarSenha, cancellationToken))
            {
                return true;
            }

            Notificar($"Ocorreu um erro efetuar o reset de senha do e-mail: {resetarSenha.Email}");
            return false;
        }

        public async Task<TokenUsuario?> IdentificarAdminAsync(string email, string senha, CancellationToken cancellationToken) =>
            await cognitoGateway.IdentifiqueSe(email, null, senha, cancellationToken);
    }
}
