using Core.Domain.Notificacoes;
using Core.WebApi.Controller;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using UseCases.Models.Request;
using UseCases.UseCases;

namespace Api.Controllers
{
    [Route("produtos")]
    public class ProdutosController(IProdutoUseCase produtoUseCase, INotificador notificador) : MainController(notificador)
    {
        [HttpGet]
        public async Task<IActionResult> ObterTodosProdutos(CancellationToken cancellationToken)
        {
            var result = await produtoUseCase.ObterTodosProdutosAsync(cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpGet("categoria")]
        public async Task<IActionResult> ObterProdutosCategoria(Categoria categoria, CancellationToken cancellationToken)
        {
            var result = await produtoUseCase.ObterProdutosCategoriaAsync(categoria, cancellationToken);

            return CustomResponseGet(result);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProduto(ProdutoRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            var result = await produtoUseCase.CadastrarProdutoAsync(request, cancellationToken);

            return CustomResponsePost($"produtos/{request.Id}", request, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> AtualizarProduto([FromRoute] Guid id, ProdutoRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return ErrorBadRequestModelState(ModelState);
            }

            if (id != request.Id)
            {
                return ErrorBadRequestPutId();
            }

            var result = await produtoUseCase.AtualizarProdutoAsync(request, cancellationToken);

            return CustomResponsePutPatch(request, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletarProduto([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await produtoUseCase.DeletarProdutoAsync(id, cancellationToken);

            return CustomResponseDelete(id, result);
        }
    }
}
