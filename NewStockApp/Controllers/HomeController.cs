  using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Products;
using WebApp.StockApp.MiddleWares;

namespace DatabaseFirstExample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService _service, ICategoryService categoryService)
        {
            this._productService = _service;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(FilterProductsViewModel filter)
        {
            ViewBag.Categories = await _categoryService.GetAllViewModel();
            var list = await _productService.GetAllViewModelWithFilter(filter);
            return View(list);
        }          
    }
}
