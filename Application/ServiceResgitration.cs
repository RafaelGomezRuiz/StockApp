using Application.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockApp.Core.Application.Interfaces.Services;
using System.Reflection;

namespace StockApp.Infraestructure.Persistence
{
    public static class ServiceResgitration
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //there are to especify the asembly
            services.AddMediatR(Assembly.GetExecutingAssembly());

            #region Services
            //services.AddTransient(typeof(IGenericService<SaveViewModel,ViewModel>),typeof(GenericService<SaveViewModel, ViewModel>));
            services.AddTransient<IProductService, ProductService>();
                services.AddTransient<ICategoryService, CategoryService>();
                services.AddTransient<IUserService, UserService>();
            #endregion
        }
    }
}
