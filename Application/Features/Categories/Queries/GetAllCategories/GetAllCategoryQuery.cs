using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Categories;

namespace StockApp.Core.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoryQuery : IRequest<IEnumerable<CategoryViewModel>>
    {
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<CategoryViewModel>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryViewModel>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categoriesVm = await GetAllViewModelWithInclude();
            if (categoriesVm == null) throw new Exception("There are not categories");

            return categoriesVm;
        }

        private async Task<List<CategoryViewModel>> GetAllViewModelWithInclude()
        {
            var categorylist = await categoryRepository.GetAllWithIncludeAsync(new List<string> { "Products" });
            
            return categorylist.Select(category => new CategoryViewModel
            {
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
                ProductsQuantity = category.Products.Count()
                //ProductsQuantity = (userViewModel != null ? s.Products.Where(product => product.UserId == userViewModel.Id).Count() : s.Products.Count())
            }).ToList();
        }
    }
}
