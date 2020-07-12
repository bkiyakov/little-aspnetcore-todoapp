using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreTodo
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services){
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            await EnsureTestAdminAsync(userManager);
        }

        private static async Task EnsureTestAdminAsync(UserManager<IdentityUser> userManager)
        {
            var testAdmin = await userManager.Users.Where(user => user.UserName == Constants.TestAdminUsername).SingleOrDefaultAsync();

            if (testAdmin != null) return;

            testAdmin = new IdentityUser(){
                UserName = Constants.TestAdminUsername,
                Email = Constants.TestAdminEmail
            };

            await userManager.CreateAsync(testAdmin, Constants.TestAdminPassword);
            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);

            // Подтверждение почты
            var emailConfirmToken = await userManager.GenerateEmailConfirmationTokenAsync(testAdmin);
            await userManager.ConfirmEmailAsync(testAdmin, emailConfirmToken);
            
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }
    }
}