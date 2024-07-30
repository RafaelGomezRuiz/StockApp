using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Domain.Entities;

namespace StockApp.Core.Application.Interfaces.Services
{
    public interface ICategoryService : IGenericService<SaveCategoryViewModel, CategoryViewModel, Category>
    {
        Task<List<CategoryViewModel>> GetAllViewModelWithInclude();
    }
}