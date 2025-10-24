using Microsoft.AspNetCore.Identity;


namespace Identity.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
