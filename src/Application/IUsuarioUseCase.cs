using Domain.Entities;

namespace UseCases
{
    public interface IUsuarioUseCase
    {
        Task<bool> CadastrarUsuarioAsync(Usuario usuario, string senha, CancellationToken cancellationToken);
    }
}