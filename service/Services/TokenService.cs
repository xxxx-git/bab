using Shared;
using Shared.Config;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Services {
    public class SynTokenService : ISecurityTokenService
    {
        private readonly ITokenSettings _tokenSettings;

        public SynTokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public string Generate(string user)
        {
            var token = GenerateToken(user);
            return token;
        }

        public string Verify(string token)
        {
            var validationParameters = new TokenValidationParameters 
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetKey(_tokenSettings.Secret),
                ValidateAudience = false,
                ValidateIssuer = false
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var claims = principal.Claims.ToDictionary(
                claim => claim.Type);
            
            var user = claims["user"].Value;

            return user;
        }

        private object ValidateToken(string token)
        {
            return null;
        }

        private string GenerateToken(string user) 
        {
            var claims = GetClaims(user);
            var signature = GetSignature();
             
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                // EncryptingCredentials = new EncryptingCredentials(secret.Key, SecurityAlgorithms.HmacSha256, SecurityAlgorithms.RsaSha256),
                SigningCredentials = signature,
                Subject = claims,
            };

            // Handle token descriptor by JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenObject = tokenHandler.CreateToken(tokenDescriptor);
            tokenObject.SigningKey = signature.Key;

            // Exract token
            var token = tokenHandler.WriteToken(tokenObject);

            return token;
        }

        private ClaimsIdentity GetClaims(string user) 
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, "Syn"),
                new Claim(JwtRegisteredClaimNames.Aud, "User"),
                new Claim(JwtRegisteredClaimNames.Sub, "token subject"),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMonths(0).AddDays(0).AddHours(1).ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user", user)
            };

            var payload = new ClaimsIdentity(claims);

            return payload;
        }

        private SecurityKey GetKey(string secretKey)
        {
            var encodedBytes = Encoding.UTF8.GetBytes(secretKey);
            var encodedSecret = Convert.ToBase64String(encodedBytes);
            return new SymmetricSecurityKey(encodedBytes);
        }
        
        private SigningCredentials GetSignature() 
        {
            var key = GetKey(_tokenSettings.Secret);
            var secret = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return secret;
        }
}
    }
