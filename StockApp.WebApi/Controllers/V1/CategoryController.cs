using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Features.Categories.Commands.CreateCategory;
using StockApp.Core.Application.Features.Categories.Commands.DeleteCategory;
using StockApp.Core.Application.Features.Categories.Commands.UpdateCategory;
using StockApp.Core.Application.Features.Categories.Queries.GetAllByIdCategories;
using StockApp.Core.Application.Features.Categories.Queries.GetAllCategories;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.ViewModels.Categories;
using StockApp.Core.Application.ViewModels.Products;

namespace StockApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles ="BASIC")]
    public class CategoryController : BaseApiController
    {
        //protected readonly ICategoryService _categoryService;
        //public CategoryController(ICategoryService _categoryService)
        //{
        //    this._categoryService = _categoryService;
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllCategoryQuery()));
                //IEnumerable<CategoryViewModel> categories = await _categoryService.GetAllViewModelWithInclude();
                //if (categories == null || categories.Count() == 0)
                //{
                //    return NotFound();
                //}
                //return Ok(categories);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetCategoryByIdQuery() { Id = id }));
                //SaveCategoryViewModel category = await _categoryService.GetByIdSaveViewModel(id);
                //if (category == null )
                //{
                //    return NotFound();
                //}
                //return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Post(CreateCategoryCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(await Mediator.Send(command));
                //await _categoryService.Add(vm);
                //return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
}

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveCategoryViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Put(int id, UpdateCategoryCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (id != command.Id)
                    return BadRequest();

                return Ok(await Mediator.Send(command));
                //await _categoryService.Update(vm, id);
                //return Ok(vm);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(DeleteCategoryByIdCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
                //await _categoryService.Delete(id);
                //return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
