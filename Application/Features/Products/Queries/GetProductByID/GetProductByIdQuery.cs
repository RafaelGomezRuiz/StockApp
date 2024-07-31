using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;

namespace StockApp.Core.Application.Features.Products.Queries.GetProductByID
{
    public class GetProductByIdQuery : IRequest<ProductViewModel>
    {
        public int Id { get; set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewModel>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);
            var productVm= mapper.Map<ProductViewModel>(product);

            return (productVm == null) ? throw new Exception("Product Doesnt exits") : productVm;

        }

        private async Task<ProductViewModel> GetProductViewModel(int id)
        {
            var productlist = await productRepository.GetAllWithIncludeAsync(new List<string> { "Category" });
            var producto = productlist.FirstOrDefault(product => product.Id == id);

            ProductViewModel productVm = new()
            {
                Name = producto.Name,
                Description = producto.Description,
                Id = producto.Id,
                Price = producto.Price,
                ImagePath = producto.ImagePath,
                CategoryName = producto.Category.Name,
                CategoryId = producto.CategoryId,
                Category = mapper.Map<CategoryViewModel>(producto.Category),
                UserId = producto.UserId
            };

            return productVm;
        }
    }
}
