using System.Collections.Generic;
using System.Security.Claims;

namespace Blog.Auth.Abstractions
{
    public interface IJwtService
    {
        bool IsTokenValid(JwtToken token);
        JwtToken GenerateToken(params Claim[] claims);
        IEnumerable<Claim> GetTokenClaims(JwtToken token);
    }
}
