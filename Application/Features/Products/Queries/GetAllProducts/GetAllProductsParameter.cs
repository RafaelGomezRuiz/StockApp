using Swashbuckle.AspNetCore.Annotations;

namespace StockApp.Core.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// parameters to filter a product
    /// </summary>
    public class GetAllProductsParameter
    {
        [SwaggerParameter(Description = "Write the id of the category to filter by")]
        public int  CategoryId { get; set; }
    }
}
