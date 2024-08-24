using AutoMapper;
using MediatR;
using StockApp.Core.Application.Exceptions;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace StockApp.Core.Application.Features.Products.Queries.GetAllProducts
{
    //request
    /// <summary>
    /// parameters to filter a product
    /// </summary>
    public class GetAllProductsQuery : IRequest<Response<List<ProductViewModel>>>
    {
        [SwaggerParameter(Description = "Write the id of the category to filter by")]
        public int CategoryId { get; set; }
    }

    //request ahndler
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Response<List<ProductViewModel>>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<Response<List<ProductViewModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productsList = await GetAllViewModelWithFilter(request);
            var reponse = new Response<List<ProductViewModel>>() { Data=productsList };
            return (productsList == null || productsList.Count == 0) ? throw new ApiException("Products not found", (int)HttpStatusCode.NotFound) : reponse;
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
