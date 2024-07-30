using Microsoft.AspNetCore.Identity;
using StockApp.Core.Application.Enums;
using StockApp.Infraestructure.Identity.Entities;

namespace StockApp.Infraestructure.Identity.Seeds
{
    public static class DefaultSuperAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultBasicUser = new();
            defaultBasicUser.UserName = "SuperAdminUser";
            defaultBasicUser.Email = "BasicUser@gmail.com";
            defaultBasicUser.FirstName = "Johns";
            defaultBasicUser.LastName = "Does";
            defaultBasicUser.EmailConfirmed = true;
            defaultBasicUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != defaultBasicUser.Id))
            {
                var user=await userManager.FindByEmailAsync(defaultBasicUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultBasicUser, "143Pa$$word!");
                    await userManager.AddToRoleAsync(defaultBasicUser, Roles.BASIC.ToString());
                    await userManager.AddToRoleAsync(defaultBasicUser, Roles.ADMIN.ToString());
                    await userManager.AddToRoleAsync(defaultBasicUser, Roles.SUPERADMIN.ToString());
                }
            }
        }
    }
}
