using Microsoft.AspNetCore.Identity;
using InventoryAPI.Models;

namespace InventoryAPI.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        string[] roles = { "Admin", "User" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (roleResult.Succeeded)
                {
                    Console.WriteLine($"✅ Role '{role}' created.");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create role '{role}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️ Role '{role}' already exists.");
            }
        }

        var adminEmail = "admin@ims.com";
        var adminPassword = "Momloves@25";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            Console.WriteLine("🔄 Seeding admin user...");

            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ Admin user created and assigned to Admin role.");
            }
            else
            {
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"❌ Admin creation error: {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine("ℹ️ Admin user already exists.");
        }
    }
}
