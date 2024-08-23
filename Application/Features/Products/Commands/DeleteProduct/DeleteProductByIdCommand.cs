using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Products.Commands.DeleteProduct
{
    /// <summary>
    /// Necessary parameters to delete a product
    /// </summary>
    public class DeleteProductByIdCommand : IRequest<int>
    {
        [SwaggerParameter(Description ="Id of the product to delete")]
        public int Id { get; set; }
    }

    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductByIdCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<int> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
        {
            var producto = await productRepository.GetByIdAsync(command.Id);

            if (producto == null) throw new Exception("Product Doesnt exits");


            await productRepository.DeleteAsync(producto);

            return producto.Id;
        }
    }

}
