﻿using Core.Domain.Entities;

namespace Infra.Dto
{
    public class ClienteDb : Entity
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}
