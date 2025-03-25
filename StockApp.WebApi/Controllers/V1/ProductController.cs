using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Features.Products.Commands.CreateProduct;
using StockApp.Core.Application.Features.Products.Commands.DeleteProduct;
using StockApp.Core.Application.Features.Products.Commands.UpdateProduct;
using StockApp.Core.Application.Features.Products.Queries.GetAllProducts;
using StockApp.Core.Application.Features.Products.Queries.GetProductByID;
using StockApp.Core.Application.ViewModels.Products;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace StockApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "ADMIN")]
    [SwaggerTag("Products Maintance")]
    public class ProductController : BaseApiController
    {
        //protected readonly IProductService _productService;
        //public ProductController(IProductService _productService)
        //{
        //    this._productService = _productService;
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List of products",
            Description = "get all products filtered by category"
        )]

        public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filters)
        {
                return Ok(await Mediator.Send(new GetAllProductsQuery() { CategoryId = filters.CategoryId }));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Product by id",
            Description = "get a product filtering by its id"
        )]

        public async Task<IActionResult> Get(int id)
        {
                return Ok(await Mediator.Send(new GetProductByIdQuery() { Id = id }));
                //SaveProductViewModel product = await _productService.GetByIdSaveViewModel(id);
                //if (product == null )
                //{
                //    return NotFound();
                //}
                //return Ok(product);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "create products",
            Description = "Receibe the necessary parameters to create a need product"
        )]

        public async Task<IActionResult> Post([FromBody] CreateProductCommand command)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(Mediator.Send(command));
                //await _productService.Add(vm);
                //return NoContent();
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveProductViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update products",
            Description = "Receibe the necessary parameters to update an available product"
        )]

        public async Task<IActionResult> Put(int id,[FromBody] UpdateProductCommand command)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                //ToString validate the prodtc to update
                if (id != command.Id)
                {
                    return BadRequest();
                }

                return Ok(await Mediator.Send(command));

                //await _productService.Update(vm, id);
                //return Ok(vm);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "delete product",
            Description = "Receibe the id to delete  an available product"
        )]
        public async Task<IActionResult> Delete(int id)
        {
                await Mediator.Send(new DeleteProductByIdCommand() { Id = id });
                return NoContent();

                //await _productService.Delete(id);
                //return NoContent();
        }
    }
}
