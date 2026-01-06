using Microsoft.AspNetCore.Identity;

namespace TASKDITASK.Models;

public class AppUser:IdentityUser
{
    public string FullName { get; set; }
}
