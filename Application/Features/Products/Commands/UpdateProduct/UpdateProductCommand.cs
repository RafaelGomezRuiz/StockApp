using AutoMapper;
using MediatR;
using StockApp.Core.Application.Exceptions;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Application.Wrappers;
using StockApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace StockApp.Core.Application.Features.Products.Commands.UpdateProduct
{
    /// <summary>
    /// Necessary parameters to update a product
    /// </summary>
    public class UpdateProductCommand : IRequest<Response<ProductUpdateResponse>>
    {
        [SwaggerParameter(Description = "Id of the product to update")]
        public int Id { get; set; }
        
        [SwaggerParameter(Description = "The name to update")]
        public string Name { get; set; }
    
        [SwaggerParameter(Description = "Description of the product to update")]
        public string Description { get; set; }

        [SwaggerParameter(Description = "Select a new image")]
        public string? ImagePath { get; set; }

        [SwaggerParameter(Description = "Price updated")]
        public double Price { get; set; }
        
        [SwaggerParameter(Description = "If of the new category")]
        public int CategoryId { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<ProductUpdateResponse>> 
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
        }

        public async Task<Response<ProductUpdateResponse>> Handle (UpdateProductCommand command, CancellationToken cancellationToken) 
        {
            var product = await productRepository.GetByIdAsync(command.Id);
            if (product == null) throw new ApiException("Product not found", (int)HttpStatusCode.NotFound);

            product = mapper.Map<Product>(command);
            await productRepository.UpdateAsync(product, product.Id);
            var productResponse =  mapper.Map<ProductUpdateResponse>(product);

            return new Response<ProductUpdateResponse>(productResponse);
        }
    }
}
