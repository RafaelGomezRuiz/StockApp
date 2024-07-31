using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;

namespace StockApp.Core.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
    }

    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductByIdCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        public Task<int> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
