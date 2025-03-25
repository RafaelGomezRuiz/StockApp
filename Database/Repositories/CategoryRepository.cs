using Application.Repository;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using StockApp.Infraestructure.Persistence.Context;

namespace StockApp.Infraestructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationContext _dbContext) : base(_dbContext)
        {
        }
    }
}
