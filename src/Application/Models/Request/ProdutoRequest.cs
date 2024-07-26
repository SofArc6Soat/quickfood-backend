using Core.Domain.DataAnnotations;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Models.Request
{
    public record ProdutoRequest
    {
        [RequiredGuid(ErrorMessage = "O campo {0} é obrigatório.")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(200, ErrorMessage = "O campo {0} deve conter entre {2} e {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, 9999, ErrorMessage = "O campo {0} deve ter o valor entre {1} e {2}.")]
        [Display(Name = "Preço")]
        public required decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public required bool Ativo { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [EnumDataType(typeof(Categoria))]
        public required Categoria Categoria { get; set; }
    }
}
