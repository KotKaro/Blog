using System;
using System.Security.Claims;
using Blog.Auth.Abstractions;

namespace Blog.Auth
{
    public class ClaimFactory : IClaimFactory
    {
        public Claim CreateUserClaim(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(userName));
            }

            return new Claim(ClaimNames.Username, userName);
        }
    }
}
