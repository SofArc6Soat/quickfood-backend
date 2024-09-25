using Core.Domain.Base;
using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;

namespace UseCases
{
    public class UsuarioUseCase(IUsuarioGateway usuarioGateway, INotificador notificador) : BaseUseCase(notificador), IUsuarioUseCase
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
    }
}
