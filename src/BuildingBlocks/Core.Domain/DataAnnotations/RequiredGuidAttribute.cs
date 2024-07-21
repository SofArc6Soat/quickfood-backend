using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DataAnnotations
{
    public class RequiredGuidAttribute : ValidationAttribute
    {
        public RequiredGuidAttribute() => ErrorMessage = "{0} é obrigatório.";

#pragma warning disable CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
        public override bool IsValid(object value)
#pragma warning restore CS8765 // A nulidade do tipo de parâmetro não corresponde ao membro substituído (possivelmente devido a atributos de nulidade).
            => value != null && value is Guid && !Guid.Empty.Equals(value);
    }
}
