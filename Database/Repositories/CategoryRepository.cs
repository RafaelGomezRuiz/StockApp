using Application.Repository;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Core.Domain.Entities;
using StockApp.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Infraestructure.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        protected readonly ApplicationContext _dbContext;
        public CategoryRepository(ApplicationContext _dbContext) :base(_dbContext)
        {
            this._dbContext = _dbContext;
        }
    }
}
