using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;

namespace StockApp.Core.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? UserId { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancelationToken)
        {
            var product = mapper.Map<Product>(command);
            product = await productRepository.AddAsync(product);
            return product.Id;
        }
    }

    
}
