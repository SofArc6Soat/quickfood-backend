using Domain.Entities;
using Domain.ValueObjects;
using FluentValidation.TestHelper;

namespace Domain.Tests.Validators;

public class ValidarPedidoTests
{
    private readonly ValidarPedido _validator;

    public ValidarPedidoTests()
    {
        _validator = new ValidarPedido();
    }

    [Fact]
    public void ValidarPedido_DeveSerValido_QuandoTodosOsCamposEstaoCorretos()
    {
        // Arrange
        var pedido = new Pedido(Guid.NewGuid(), 1, Guid.NewGuid(), PedidoStatus.Rascunho, 100.00m, DateTime.Now);

        // Act
        var result = _validator.TestValidate(pedido);

        // Assert
        result.ShouldNotHaveValidationErrorFor(p => p.Id);
        result.ShouldNotHaveValidationErrorFor(p => p.NumeroPedido);
        result.ShouldNotHaveValidationErrorFor(p => p.ClienteId);
        result.ShouldNotHaveValidationErrorFor(p => p.ValorTotal);
        result.ShouldNotHaveValidationErrorFor(p => p.DataPedido);
    }
/*
 * 
 * teste
    [Fact]
    public void ValidarPedido_DeveTerErro_QuandoCamposNaoEstaoCorretos()
    {
        // Arrange
        var pedido = new Pedido(Guid.Empty, 1, Guid.NewGuid(), PedidoStatus.Rascunho, 100.00m, DateTime.Now);

        var validator = new ValidarPedido();

        // Act
        var result = validator.TestValidate(pedido);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Id)
              .WithErrorMessage("O Id deve ser válido.");
        result.ShouldHaveValidationErrorFor(p => p.NumeroPedido)
              .WithErrorMessage("O Número do Pedido deve ser maior que zero.");
        result.ShouldHaveValidationErrorFor(p => p.ClienteId)
              .WithErrorMessage("O ClienteId deve ser válido.");
        result.ShouldHaveValidationErrorFor(p => p.Status)
              .WithErrorMessage("O Status deve ser válido.");
        result.ShouldHaveValidationErrorFor(p => p.ValorTotal)
              .WithErrorMessage("O Valor Total deve ser maior que zero.");
        result.ShouldHaveValidationErrorFor(p => p.DataPedido)
              .WithErrorMessage("A Data do Pedido não pode ser no futuro.");
    }
*/
}