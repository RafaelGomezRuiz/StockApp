using Application.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockApp.Core.Application.Interfaces.Repositories;
using StockApp.Infraestructure.Persistence.Context;
using StockApp.Infraestructure.Persistence.Repositories;

namespace StockApp.Infraestructure.Persistence
{
    public static class ServiceResgitration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context

            if (configuration.GetValue<bool>("UseIUnMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("StockAppInMemory"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(options =>
                                            options.UseSqlServer(connectionString , a => a.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
            }
            #endregion
             
            #region Repositories
            //services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            #endregion
        }
    }
}
