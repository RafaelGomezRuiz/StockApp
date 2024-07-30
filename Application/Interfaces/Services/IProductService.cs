using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Domain.Entities;

namespace StockApp.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel,Product>
    {
        Task<List<ProductViewModel>> GetAllViewModelWithFilter(FilterProductsViewModel filters);
        Task<List<ProductViewModel>> GetAllViewModelWithInclude();
    }
}