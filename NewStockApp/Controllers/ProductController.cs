using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Products;

namespace DatabaseFirstExample.Controllers
{
    [Authorize(Roles ="BASIC")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService _productService, ICategoryService categoryService)
        {
            this._productService = _productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _productService.GetAllViewModelWithInclude();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            SaveProductViewModel vm = new();
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveProduct", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _categoryService.GetAllViewModel();   
                return View("SaveProduct", vm);
            }

            SaveProductViewModel productVm = await _productService.Add(vm);
            if (productVm != null || productVm.Id!=0)
            {
                productVm.ImagePath=UploadFile(vm.File, productVm.Id);
                await _productService.Update(productVm,productVm.Id);   
            }

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            SaveProductViewModel vm = await _productService.GetByIdSaveViewModel(id);
            vm.Categories = await _categoryService.GetAllViewModel();
            return View("SaveProduct", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _categoryService.GetAllViewModel();
                return View("SaveProduct", vm);
            }
            SaveProductViewModel productVm = await _productService.GetByIdSaveViewModel(vm.Id);
            vm.ImagePath=UploadFile(vm.File, productVm.Id, true, productVm.ImagePath);

            await _productService.Update(vm,vm.Id);
            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await _productService.GetByIdSaveViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            string basePath = $"/Images/Products/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            if (Directory.Exists(path))
            { 
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                //Para borrar los folder que tengan los archivos dentro
                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }
                //sizeof no hacemos lo anterior esto da error
                Directory.Delete(path);
            }
                await _productService.Delete(id);

            return RedirectToRoute(new { controller = "Product", action = "Index" });
        }

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imagePath = "")
        {
            if (isEditMode  && file==null)
            {
                return imagePath;
            }
            string basePath = $"/Images/Products/{id}";
            //FormatException correcta de combinar dos rutas
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //Create folder if not exists
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //GetHashCode file path
            Guid guid = Guid.NewGuid(); //NombreUnico
            FileInfo fileInfo = new (file.FileName);
            string fileName = guid + fileInfo.Extension;

            string  fileNameWithPath = Path.Combine(path, fileName);
            //subirlo
            //FileStream un string para manipular archivos
            using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                //Este archvivo que el usuairo subio se copiara en el fileNameWithPath, que es el folder fisico que le enidque 
                file.CopyTo(stream);
            }

            if(isEditMode)
            {
                string[] oldImagePart = imagePath.Split('/');
                string oldImageName = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImageName);
                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            //retornar la url que guardaremos en la imagen
            return $"{basePath}/{fileName}";
        }
    }
}
