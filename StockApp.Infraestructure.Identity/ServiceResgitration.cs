using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StockApp.Core.Application.Interfaces.Services;
using StockApp.Core.Application.Wrappers;
using StockApp.Core.Domain.Settings;
using StockApp.Infraestructure.Identity.Entities;
using StockApp.Infraestructure.Identity.Seeds;
using StockApp.Infraestructure.Identity.Services;
using StockApp.Infraestructure.Persistence.Context;
using System.Text;

namespace StockApp.Infraestructure.Identity
{
    public static class ServiceResgitration
    {
        public static void AddIdentityInfraestructureForApi(this IServiceCollection services, IConfiguration configuration)
        {
            ContextConfiguration(services, configuration);

            #region Identity

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Arent authorized"));
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Arent authorized to access these resources"));
                        return c.Response.WriteAsync(result);
                    }
                };
            });
            #endregion

            #region Services
            ServiceConfiguration(services);
            #endregion
        }

        //ForWeb
        public static void AddIdentityInfraestructureForWeb(this IServiceCollection services, IConfiguration configuration)
        {
            ContextConfiguration(services, configuration);

            #region Identity
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

                services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/User";
                    options.AccessDeniedPath = "/User/AccessDenied";
                });
                
                //sin esta linea daba error porque como ambos sevices utilizan accoutn service, se genera el codigo de JWT 
                // la solucinon ideal es crear un factory para que devuelva el account service requerido
                services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));

                services.AddAuthentication();
            #endregion

            #region Services
            ServiceConfiguration(services);
            #endregion
        }

        private static void ContextConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            #region Context

            if (configuration.GetValue<bool>("UseIUnMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityContextInMemory"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("IdentityConnection");
                services.AddDbContext<IdentityContext>(options =>
                                            options.UseSqlServer(connectionString, a => a.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }
            #endregion
        }
        private static void ServiceConfiguration(IServiceCollection services)
        {
            #region Services
                services.AddTransient<IAccountService, AccountService>();
            #endregion
        }

        public static async Task AddIdentitySeeds(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);
                    await DefaultBasicUser.SeedAsync(userManager);
                    await DefaultSuperAdminUser.SeedAsync(userManager);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
