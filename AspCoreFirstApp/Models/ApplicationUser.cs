using Microsoft.AspNetCore.Identity;

namespace AspCoreFirstApp.Models;

public class ApplicationUser : IdentityUser
{
    public string? City { get; set; }
}
