using Blog.Auth.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Auth.Models
{
    public class JwtContainerModel : IAuthContainerModel
    {
        public int ExpireMinutes { get; set; } = 30;
        public string SecretKey { get; set; } = "MTIzZXJFV3dlLnF3ZS5xd0UhQCMuMTI/Iw=="; // This secret key should be moved to some configurations outter server.
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
    }
}
