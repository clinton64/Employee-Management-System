using EMS.Models;
using EMS.Enums;
using Microsoft.AspNetCore.Identity;

public static class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<Employee> userManager, RoleManager<UserRoles> roleManager)
    {
        //Seed Roles
        // write a code to add UserRoles in database
        await roleManager.CreateAsync(new UserRoles { Name = UserRoles.SuperAdmin, NormalizedName = UserRoles.SuperAdmin });
        await roleManager.CreateAsync(new UserRoles { Name = UserRoles.Admin, NormalizedName = UserRoles.Admin });
        await roleManager.CreateAsync(new UserRoles { Name = UserRoles.TeamLead, NormalizedName = UserRoles.TeamLead });
        await roleManager.CreateAsync(new UserRoles { Name = UserRoles.Employee, NormalizedName = UserRoles.Employee });
       
    }
    
    // write a function to add an employee in database which user role is SuperAdmin
    public static async Task SeedSuperAdminAsync(UserManager<Employee> userManager, RoleManager<UserRoles> roleManager)
    {
        // create an employee object here and set all the properties
        var defaultUser = new Employee
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            EmployeeName = "Admin",
            JobTitle = "Admin",
            BloodGroup = BloodGroup.APositive,
            CreatedAt = DateTime.Now,
            LastUpdatedAt = DateTime.Now,
            Id = Guid.NewGuid()
        };
        // write a code to add this employee in database
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Admin@123");
                await userManager.AddToRoleAsync(defaultUser, UserRoles.SuperAdmin);
            }
        }
    }
}