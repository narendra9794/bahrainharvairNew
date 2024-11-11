using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class DataSeed
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync(Constants.SuperAdmin))
        {
            await roleManager.CreateAsync(new IdentityRole(Constants.SuperAdmin));
        }

        var superAdmins = new List<ApplicationUser>()
                {
                    new ApplicationUser
                    {
                        UserName = "shubham.m@mishainfotech.com",
                        Email = "shubham.m@mishainfotech.com",
                        FirstName = "Shubham",
                        LastName = "Gupta",
                        Role = Constants.SuperAdmin,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = true
                    },
                    new ApplicationUser
                    {
                        UserName = "rashi.k@mishainfotech.com",
                        Email = "rashi.k@mishainfotech.com",
                        FirstName = "Rashi",
                        LastName = "Kumar",
                        Role = Constants.SuperAdmin,
                        CreatedOn = DateTime.UtcNow,
                        IsActive = true
                    }
                };


        foreach (var superAdmin in superAdmins)
        {
            var existingUser = await userManager.FindByEmailAsync(superAdmin.Email);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(superAdmin, "Misha@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, Constants.SuperAdmin);
                }
                else
                {
                    // Log errors or handle them
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {superAdmin.Email}: {error.Description}");
                    }
                }
            }
        }
    }
}
