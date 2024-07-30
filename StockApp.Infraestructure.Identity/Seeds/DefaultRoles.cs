using Microsoft.AspNetCore.Identity;
using StockApp.Core.Application.Enums;

namespace StockApp.Infraestructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole (Roles.SUPERADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.BASIC.ToString()));
        }
    }
}
