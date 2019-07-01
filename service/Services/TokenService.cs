using Shared;
using Shared.Config;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services {
    public class SynTokenService : ISecurityTokenService
    {
        private readonly ITokenSettings _tokenSettings;

        public SynTokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        public string GenerateToken(IUser user)
        {
            var token = GetToken(user);
            

            return token;
        }

        private ClaimsIdentity GetClaims(IUser user) 
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, "Syn"),
                new Claim(JwtRegisteredClaimNames.Aud, "User"),
                new Claim(JwtRegisteredClaimNames.Sub, "token subject"),
                new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(5).ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id),
                new Claim("userHierarchy", user.Hierarchy),
                new Claim("userDisplayName", user.DisplayName)
            };

            var payload = new ClaimsIdentity(claims);

            return payload;
        }

        private SigningCredentials GetSignature() 
        {
            var encodedBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var encodedSecret = Convert.ToBase64String(encodedBytes);
            var secret = new SigningCredentials(new SymmetricSecurityKey(encodedBytes), SecurityAlgorithms.HmacSha256);
            return secret;
        }

        private string GetToken(IUser user) 
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
    }
}