
using BlazorAppMSAuth.Data;
using Microsoft.AspNetCore.Identity;

namespace BlazorAppMSAuth.Configuration;

public static class IdentityDataSetup
{
    public static async Task SeedData(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
    {
        // Seed roles
        string[] roleNames = configuration.GetSection("Roles").Get<string[]>();
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Check the default user and create if not exist
        var defaultUserEmail = configuration["DefaultUser:Email"];
        var defaultUserPassword = configuration["DefaultUser:Password"];
        var user = await userManager.FindByEmailAsync(defaultUserEmail);
        if (user == null)
        {
            user = new ApplicationUser { UserName = defaultUserEmail, Email = defaultUserEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, defaultUserPassword);
            if (result.Succeeded)
            {
                // Assign roles to the user here, for example, assigning the 'Admin' role
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
        else if (!user.EmailConfirmed)
        {
            // If the user already exists but the email is not confirmed, confirm it
            user.EmailConfirmed = true;
            await userManager.UpdateAsync(user);
        }
    }
}
