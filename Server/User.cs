using Microsoft.AspNetCore.Identity;

namespace HammerProject.Server
{
    public class User: IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
