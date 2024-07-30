using MediatR;
using StockApp.Core.Application.ViewModels.Products;

namespace StockApp.Core.Application.Features.Products.Queries.GetAllProducts
{
    //request
    public class GetAllProductsQuery : IRequest<IList<ProductViewModel>>
    {
        public int CategoryId { get; set; }
    }

    //request ahndler
    //public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IList<ProductViewModel>>
    //{
        
    //}
}
