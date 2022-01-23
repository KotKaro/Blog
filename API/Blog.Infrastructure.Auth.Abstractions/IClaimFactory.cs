using System.Security.Claims;

namespace Blog.Auth.Abstractions
{
    public interface IClaimFactory
    {
        Claim CreateUserClaim(string userDetailsUsername);
    }
}
