using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Domain.Entities;

namespace StockApp.Core.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductUpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? UserId { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand,ProductUpdateResponse> 
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
        }

        public async Task<ProductUpdateResponse> Handle (UpdateProductCommand command, CancellationToken cancellationToken) 
        {
            var product = await productRepository.GetByIdAsync(command.Id);

            if (product == null) throw new Exception("Product Doesnt exits");

            product = mapper.Map<Product>(command);

            await productRepository.UpdateAsync(product, product.Id);

            var productResponse =  mapper.Map<ProductUpdateResponse>(product);
            
            return productResponse;
        }
    }
}
