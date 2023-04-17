using Microsoft.AspNetCore.Identity;

namespace EMS.Models;

public class UserRoles : IdentityRole<Guid>
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Admin = "Admin";
    public const string TeamLead = "TeamLead";
    public const string Employee = "Employee";
}