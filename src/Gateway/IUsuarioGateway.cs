using Domain.Entities;

namespace Gateways
{
    public interface IUsuarioGateway
    {
        bool VerificarUsuarioExistente(Guid id, string? email, CancellationToken cancellationToken);
        Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
    }
}
