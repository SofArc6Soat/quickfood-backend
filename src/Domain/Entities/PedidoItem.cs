﻿using Core.Domain.Entities;
using FluentValidation;

namespace Domain.Entities
{
    public class PedidoItem : Entity
    {
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        public PedidoItem(Guid produtoId, int quantidade, decimal valorUnitario)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public void AdicionarUnidades(int unidades)
        {
            if (unidades <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(unidades), "O número de unidades a serem adicionadas deve ser maior que zero.");
            }

            Quantidade += unidades;
        }

        public decimal CalcularValor() =>
            Quantidade * ValorUnitario;
    }

    public class ValidarPedidoItem : AbstractValidator<PedidoItem>
    {
        public ValidarPedidoItem()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.ProdutoId)
                .NotNull().WithMessage("O {PropertyName} não pode ser nulo.")
                .NotEmpty().WithMessage("O {PropertyName} deve ser válido.");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0).WithMessage("A {PropertyName} deve ser maior do que {ComparisonValue}.");

            RuleFor(c => c.ValorUnitario)
                .GreaterThan(0).WithMessage("O {PropertyName} deve ser maior do que {ComparisonValue}.");
        }
    }
}
