using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.Wrappers;
using StockApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Necessary parameters for the creation of a product
    /// </summary>
    public class CreateProductCommand : IRequest<Response<int>>
    {
        /// <example>
        /// Tostadora
        /// </example>
        [SwaggerParameter (Description = "Name of the product")]
        public string Name { get; set; }
        
        /// <example>
        /// electrica y negra
        /// </example>
        [SwaggerParameter(Description = "description of the product")]
        public string Description { get; set; }
        
        [SwaggerParameter (Description = "Image of the product")]
        public string? ImagePath { get; set; }

        [SwaggerParameter (Description = "Price of the product")]
        /// <example>
        /// 0
        /// </example>
        public double Price { get; set; }

        [SwaggerParameter (Description = "Id of the product cetegory")]
        /// <example>
        /// Id of the category asociated
        /// </example>
        public int CategoryId { get; set; }

        /// <example>
        /// "2122-sasd-33232-sw21"
        /// </example>
        [SwaggerParameter (Description = "Owner Id of the product")]
        public string? UserId { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<int>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateProductCommand command, CancellationToken cancelationToken)
        {
            var product = mapper.Map<Product>(command);
            product = await productRepository.AddAsync(product);
            return new Response<int>(product.Id);
        }
    }

    
}
