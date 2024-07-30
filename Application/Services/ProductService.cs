using AutoMapper;
using Microsoft.AspNetCore.Http;
using StockApp.Core.Application.Dtos.Account;
using StockApp.Core.Application.Helpers;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Products;
using StockApp.Core.Domain.Entities;
using System.Collections.Generic;

namespace Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel,ProductViewModel,Product>,IProductService
    {
        private readonly IProductRepository _productRepository;
        protected readonly IHttpContextAccessor _httpContextAssessor;
        protected readonly AuthenticationResponse userViewModel;
        protected readonly IMapper _mapper;

        public ProductService(IProductRepository _productRepository,IMapper _mapper, IHttpContextAccessor _httpContextAssessor) : base(_productRepository, _mapper)
        {
            this._productRepository = _productRepository;
            this._httpContextAssessor= _httpContextAssessor;
            userViewModel = _httpContextAssessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            this._mapper= _mapper;
        }

        public override async Task<SaveProductViewModel> Add(SaveProductViewModel vm)
        {
            vm.UserId = userViewModel != null ? userViewModel.Id : vm.UserId;
            return await base.Add(vm);
        }

        public override async Task Update(SaveProductViewModel vm, int id)
        {
            vm.UserId = userViewModel != null ? userViewModel.Id : vm.UserId;
            await base.Update(vm,id);
        }

        public async Task<List<ProductViewModel>> GetAllViewModelWithInclude()
        {
            var list = await _productRepository.GetAllWithIncludeAsync(new List<string> { "Category" });
            if (userViewModel !=null)
            {
                list.Where(product => product.UserId == userViewModel.Id);
            }
            return list.Select(s => new ProductViewModel
            {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                Price = s.Price,
                ImagePath = s.ImagePath,
                CategoryName=s.Category.Name,
            }).ToList();
        }

        public async Task<List<ProductViewModel>> GetAllViewModelWithFilter(FilterProductsViewModel filters)
        {
            var productlist = await _productRepository.GetAllWithIncludeAsync(new List<string> { "Category" });
            if (userViewModel != null)
            {
                productlist.Where(product => product.UserId == userViewModel.Id);
            }

            var listViewModel = productlist.Select(s => new ProductViewModel
            {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                Price = s.Price,
                ImagePath = s.ImagePath,
                CategoryName = s.Category.Name,
                CategoryId = s.CategoryId,
                UserId=s.UserId
            }).ToList();

            if(filters.CategoryId != null)
            {
                listViewModel = listViewModel.Where(product => product.CategoryId == filters.CategoryId.Value).ToList();
            }
            return listViewModel;
        }
    }
}
