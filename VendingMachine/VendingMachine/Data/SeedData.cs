using Microsoft.AspNetCore.Identity;
using VendingMachine.Models;
using static VendingMachine.Helper.Constant;

namespace VendingMachine.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { UserRole.Admin, UserRole.User };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin User
            IdentityUser user = await userManager.FindByEmailAsync("admin.vms@gmail.com");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "admin",
                    Email = "admin.vms@gmail.com",
                };
                await userManager.CreateAsync(user, "Admin@123");
            }

            await userManager.AddToRoleAsync(user, UserRole.Admin);

            // Create Regular User
            user = await userManager.FindByEmailAsync("user.vms@gmail.com");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "user",
                    Email = "user.vms@gmail.com",
                };
                await userManager.CreateAsync(user, "User@123");
            }

            await userManager.AddToRoleAsync(user, UserRole.User);
        }
    }
}
