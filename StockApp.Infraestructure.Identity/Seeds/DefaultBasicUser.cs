using Microsoft.AspNetCore.Identity;
using StockApp.Core.Application.Enums;
using StockApp.Infraestructure.Identity.Entities;

namespace StockApp.Infraestructure.Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser defaultBasicUser = new();
            defaultBasicUser.UserName = "BasicUser";
            defaultBasicUser.Email = "BasicDefaultUser@gmail.com";
            defaultBasicUser.FirstName = "John";
            defaultBasicUser.LastName = "Doe";
            defaultBasicUser.EmailConfirmed = true;
            defaultBasicUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != defaultBasicUser.Id))
            {
                var user=await userManager.FindByEmailAsync(defaultBasicUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultBasicUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultBasicUser, Roles.BASIC.ToString());
                }
            }
        }
    }
}
