using MediatR;
using StockApp.Core.Application.Exceptions;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace StockApp.Core.Application.Features.Products.Commands.DeleteProduct
{
    /// <summary>
    /// Necessary parameters to delete a product
    /// </summary>
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "Id of the product to delete")]
        public int Id { get; set; }
    }

    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductByIdCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
        {
            var producto = await productRepository.GetByIdAsync(command.Id);

            if (producto == null) throw new ApiException("Product not found", (int)HttpStatusCode.NotFound);


            await productRepository.DeleteAsync(producto);
            return new Response<int>(producto.Id);
        }
    }

}
