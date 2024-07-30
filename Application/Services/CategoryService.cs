using AutoMapper;
using Microsoft.AspNetCore.Http;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Application.ViewModels.Users;
using StockApp.Core.Domain.Entities;

namespace Application.Services
{
    public class CategoryService : GenericService<SaveCategoryViewModel,CategoryViewModel,Category>,ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        protected readonly IHttpContextAccessor _httpContextAssessor;
        protected readonly AuthenticationResponse userViewModel;
        //por si queremos hacer mapeos especiales en la entidad
        protected readonly IMapper _mapper;

        public CategoryService(ICategoryRepository _categoryRepository, IMapper _mapper, IHttpContextAccessor _httpContextAssessor) : base(_categoryRepository,_mapper)
        {
            this._categoryRepository = _categoryRepository;
            this._httpContextAssessor= _httpContextAssessor;
            userViewModel = _httpContextAssessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            this._mapper= _mapper;
        }

        //SearchOption llamaba GetAllViewModel
        public async Task<List<CategoryViewModel>> GetAllViewModelWithInclude()
        {
            var categorylist = await _categoryRepository.GetAllWithIncludeAsync(new List<string> { "Products"});
            return categorylist.Select(s => new CategoryViewModel
            {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                ProductsQuantity = (userViewModel != null ? s.Products.Where(product => product.UserId == userViewModel.Id).Count(): s.Products.Count())
            }).ToList();
        }
    }
}
