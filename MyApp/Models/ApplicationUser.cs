using Microsoft.AspNetCore.Identity;

namespace MyApp.Models // ✅ Bu namespace doğru olmalı
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
