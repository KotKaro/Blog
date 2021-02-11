using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blog.Auth.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Auth
{
    public class JwtService : IJwtService
    {
        private readonly IAuthContainerModel _authContainer;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtService(IAuthContainerModel authContainer)
        {
            _authContainer = authContainer;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public bool IsTokenValid(JwtToken token)
        {
            var tokenValidationParameters = GetTokenValidationParameters();

            try
            {
                _jwtSecurityTokenHandler.ValidateToken(token.Value, tokenValidationParameters, out _);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public JwtToken GenerateToken(params Claim[] claims)
        {
            if (claims?.Length == null || claims.Length == 0)
                throw new ArgumentException("Arguments to create token are not valid.");

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_authContainer.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), _authContainer.SecurityAlgorithm)
            };

            var securityToken = _jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return new JwtToken(_jwtSecurityTokenHandler.WriteToken(securityToken));
        }

        public IEnumerable<Claim> GetTokenClaims(JwtToken token)
        {
            var tokenValidationParameters = GetTokenValidationParameters();
            var tokenValid = _jwtSecurityTokenHandler.ValidateToken(token.Value, tokenValidationParameters, out _);
            
            return tokenValid.Claims;
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            var symmetricKey = Convert.FromBase64String(_authContainer.SecretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey()
            };
        }
    }
}