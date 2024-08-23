using AutoMapper;
using MediatR;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.ViewModels.Categories;
using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Categories.Queries.GetAllByIdCategories
{
    /// <summary>
    /// Parameters to get a category by id
    /// </summary>
    public class GetCategoryByIdQuery : IRequest<CategoryViewModel>
    {
        [SwaggerParameter(Description = "category id to get")]
        public int Id { get; set; }
    }

    public class GetCategoryByIdQueryHandle : IRequestHandler<GetCategoryByIdQuery, CategoryViewModel> 
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetCategoryByIdQueryHandle(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task<CategoryViewModel> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetAllWithIncludeAsync(new List<string> { "Products" });
            var category = categories.FirstOrDefault(category => category.Id == query.Id);
            if (category == null )throw new Exception("Category not found");
            var categoryVm = mapper.Map<CategoryViewModel>(category);
            categoryVm.ProductsQuantity = category.Products.Count;
            return categoryVm;
        }
    }
}
