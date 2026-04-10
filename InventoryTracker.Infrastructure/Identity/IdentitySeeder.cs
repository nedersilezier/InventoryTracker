using InventoryTracker.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            string[] roles = ["Admin", "User"];
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminSection = configuration.GetSection("AdminUser");
            var email = adminSection["Email"];
            var password = adminSection["Password"];
            var firstName = adminSection["FirstName"];
            var lastName = adminSection["LastName"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var existingAdmin = await userManager.FindByEmailAsync(email);

            if (existingAdmin is null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
