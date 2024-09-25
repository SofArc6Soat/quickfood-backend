using Cora.Infra.Repository;
using Infra.Context;
using Infra.Dto;

namespace Infra.Repositories
{
    public class UsuarioRepository(ApplicationDbContext context) : RepositoryGeneric<UsuarioDb>(context), IUsuarioRepository
    {
    }
}
