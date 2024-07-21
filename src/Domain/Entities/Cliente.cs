﻿using Core.Domain.Entities;
using Core.Domain.Validacoes;
using FluentValidation;

namespace Domain.Entities
{
    public class Cliente : Entity, IAggregateRoot
    {
        public string Nome { get; private set; }
        public string? Email { get; private set; }
        public string Cpf { get; private set; }
        public bool Ativo { get; private set; }

        public Cliente(Guid id, string nome, string? email, string cpf, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
            Ativo = ativo;
        }
    }

    public class ValidarCliente : AbstractValidator<Cliente>
    {
        public ValidarCliente()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.Nome)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .Length(2, 50).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("O {PropertyName} está em um formato inválido.")
                .Length(2, 100).WithMessage("O {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres e foi informado {PropertyValue}.");

            RuleFor(f => f.Cpf.Length)
                .Equal(ValidadorCpf.TamanhoCpf).WithMessage("O {PropertyName} precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
            RuleFor(f => ValidadorCpf.Validar(f.Cpf))
                .Equal(true).WithMessage("O {PropertyName} fornecido é inválido.");

            RuleFor(c => c.Ativo)
                .NotNull().WithMessage("O status não pode ser nulo.");
        }
    }
}
