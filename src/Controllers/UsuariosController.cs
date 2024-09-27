using Domain.Entities;
using Gateways.Dtos.Request;
using Gateways.Dtos.Response;
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

        public async Task<bool> ConfirmarEmailVerificacaoAsync(ConfirmarEmailVerificacaoDto confirmarEmailVerificacaoDto, CancellationToken cancellationToken)
        {
            var emailVerificacao = new EmailVerificacao(confirmarEmailVerificacaoDto.Email, confirmarEmailVerificacaoDto.CodigoVerificacao);

            return await usuarioUseCase.ConfirmarEmailVerificacaoAsync(emailVerificacao, cancellationToken);
        }

        public async Task<bool> SolicitarRecuperacaoSenhaAsync(SolicitarRecuperacaoSenhaDto solicitarRecuperacaoSenha, CancellationToken cancellationToken)
        {
            var recuperacaoSenha = new SolicitarRecuperacaoSenha(solicitarRecuperacaoSenha.Email);

            return await usuarioUseCase.SolicitarRecuperacaoSenhaAsync(recuperacaoSenha, cancellationToken);
        }

        public async Task<bool> EfetuarResetSenhaAsync(ResetarSenhaDto resetarSenhaDto, CancellationToken cancellationToken)
        {
            var resetarSenha = new ResetarSenha(resetarSenhaDto.Email, resetarSenhaDto.CodigoVerificacao, resetarSenhaDto.NovaSenha);

            return await usuarioUseCase.EfetuarResetSenhaAsync(resetarSenha, cancellationToken);
        }

        public async Task<TokenUsuario?> IdentificarAdminAsync(string email, string senha, CancellationToken cancellationToken) =>
            await usuarioUseCase.IdentificarAdminAsync(email, senha, cancellationToken);
    }
}
