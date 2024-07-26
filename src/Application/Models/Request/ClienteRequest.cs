﻿using Core.Domain.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Models.Request
{
    public record ClienteRequest
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public required string Nome { get; set; }

        [EmailAddress(ErrorMessage = "{0} está em um formato inválido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(11, ErrorMessage = "O campo {0} deve conter {1} caracteres.", MinimumLength = 11)]
        [Display(Name = "CPF")]
        public required string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}
