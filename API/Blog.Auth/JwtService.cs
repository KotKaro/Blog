using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blog.Auth.Models;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Auth
{
    public class JwtService
    {
        private readonly JwtContainerModel _authContainer;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtService(JwtContainerModel authContainer)
        {
            _authContainer = authContainer;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public bool IsTokenValid(JwtToken token)
        {
            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            try
            {
                _jwtSecurityTokenHandler.ValidateToken(token.Value, tokenValidationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public JwtToken GenerateToken(params Claim[] clamis)
        {
            if (clamis?.Length == null || clamis.Length == 0)
                throw new ArgumentException("Arguments to create token are not valid.");

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clamis),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_authContainer.ExpireMinutes)),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(), _authContainer.SecurityAlgorithm)
            };

            SecurityToken securityToken = _jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return new JwtToken(_jwtSecurityTokenHandler.WriteToken(securityToken));
        }

        public IEnumerable<Claim> GetTokenClaims(JwtToken token)
        {
            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            try
            {
                ClaimsPrincipal tokenValid = _jwtSecurityTokenHandler.ValidateToken(token.Value, tokenValidationParameters, out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(_authContainer.SecretKey);
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