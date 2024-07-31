namespace StockApp.Core.Application.Features.Products.Commands.UpdateProduct
{
    public class ProductUpdateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string? UserId { get; set; }
    }
}
