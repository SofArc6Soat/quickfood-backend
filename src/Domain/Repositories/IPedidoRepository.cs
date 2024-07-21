﻿using Core.Domain.Data;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPedidoRepository : IRepositoryGeneric<Pedido>
    {
        Task<IEnumerable<Pedido>> ObterTodosPedidosAsync();
    }
}
