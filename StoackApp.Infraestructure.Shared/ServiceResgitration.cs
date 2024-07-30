using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoackApp.Infraestructure.Shared.Services;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Domain.Settings;
using System.Reflection;

namespace StockApp.Infraestructure.Shared
{
    public static class ServiceResgitration
    {
        public static void AddSharedInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
