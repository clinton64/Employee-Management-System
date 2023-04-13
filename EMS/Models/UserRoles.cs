using Microsoft.AspNetCore.Identity;

namespace EMS.Models;

public class UserRoles : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string TeamLead = "Team Lead";
    public const string Employee = "Employee";
}