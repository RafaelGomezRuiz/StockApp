using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;

namespace StockApp.Core.Application.Features.Products.Queries.GetAllProducts
{
    //request
    public class GetAllProductsQuery : IRequest<IList<ProductViewModel>>
    {
        public int CategoryId { get; set; }
    }

    //request ahndler
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IList<ProductViewModel>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IList<ProductViewModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productsList = await GetAllViewModelWithFilter(request);
            return (productsList == null || productsList.Count == 0) ? throw new Exception("There anrent products") : productsList;
        }
        private async Task<List<ProductViewModel>> GetAllViewModelWithFilter(GetAllProductsQuery filters)
        {
            var productlist = await productRepository.GetAllWithIncludeAsync(new List<string> { "Category" });

            var listViewModel = productlist.Select(producto => new ProductViewModel
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
            }).ToList();

            if (filters.CategoryId != null)
            {
                //listViewModel = listViewModel.Where(product => product.CategoryId == filters.CategoryId.Value).ToList();
                listViewModel = listViewModel.Where(product => product.CategoryId == filters.CategoryId).ToList();

            }
            return listViewModel;
        }
    }
}
