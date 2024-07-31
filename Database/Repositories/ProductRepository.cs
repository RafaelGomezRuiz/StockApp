using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using StockApp.Infraestructure.Persistence.Context;

namespace Application.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _dbContext;

        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
