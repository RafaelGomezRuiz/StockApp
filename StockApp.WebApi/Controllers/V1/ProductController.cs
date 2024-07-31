using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.Application.Features.Products.Commands.CreateProduct;
using StockApp.Core.Application.Features.Products.Commands.DeleteProduct;
using StockApp.Core.Application.Features.Products.Commands.UpdateProduct;
using StockApp.Core.Application.Features.Products.Queries.GetAllProducts;
using StockApp.Core.Application.Features.Products.Queries.GetProductByID;
using StockApp.Core.Application.ViewModels.Products;

namespace StockApp.WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "ADMIN")]
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

        public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filters)
        {
            try
            {
                return Ok(await Mediator.Send(new GetAllProductsQuery() { CategoryId = filters.CategoryId }));
                //IEnumerable<ProductViewModel> products = await _productService.GetAllViewModelWithFilter(filters);
                //if (products == null || products.Count() == 0)
                //{
                //    return NotFound();
                //}
                //return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GetProductByIdQuery() { Id = id }));
                //SaveProductViewModel product = await _productService.GetByIdSaveViewModel(id);
                //if (product == null )
                //{
                //    return NotFound();
                //}
                //return Ok(product);
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

        public async Task<IActionResult> Post([FromBody] CreateProductCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(Mediator.Send(command));
                //await _productService.Add(vm);
                //return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveProductViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Put(int id, UpdateProductCommand command)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteProductByIdCommand() { Id = id });
                return NoContent();

                //await _productService.Delete(id);
                //return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
