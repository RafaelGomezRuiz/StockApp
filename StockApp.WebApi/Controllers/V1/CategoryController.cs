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
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace StockApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles ="BASIC")]
    [SwaggerTag("category maintance")]
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
        [SwaggerOperation(
            Summary= "Get Categories list",
            Description = "We get All categories with includes"
        )]  
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
        [SwaggerOperation(
            Summary = "Get category by id",
            Description = "We get A categories by id with includes"
        )]

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
        [SwaggerOperation(
            Summary = "post a category",
            Description = "We receibe the necessary parameters for create a category"
        )]
        [Consumes(MediaTypeNames.Application.Json)]

        public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command)
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
        [SwaggerOperation(
            Summary = "update a category",
            Description = "We receibe the necessary parameters for update a category"
        )]
        [Consumes(MediaTypeNames.Application.Json)]

        public async Task<IActionResult> Put(int id, [FromBody] UpdateCategoryCommand command)
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
        [SwaggerOperation(
            Summary = "Delete a category",
            Description = "We receibe the necessary parameters to delete a category"
        )]
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
