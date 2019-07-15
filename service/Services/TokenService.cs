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

        public string Generate(string content)
        {
            var token = GenerateToken(content);
            return token;
        }

        public string Verify(string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetKey(),
                ValidateAudience = false,
                ValidateIssuer = false
            };

            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal;
            try
            {
                 principal = handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (System.Exception)
            {
                return null;
            }

            var claims = principal.Claims.ToDictionary(
                claim => claim.Type);

            var content = claims[_tokenSettings.Claims.Content].Value;

            return content;
        }

        private object ValidateToken(string token)
        {
            return null;
        }

        private string GenerateToken(string content)
        {
            var claims = GetClaims(content);
            var signature = GetSignature();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                SigningCredentials = signature,
                Subject = claims
            };

            // Handle token descriptor by JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenObject = tokenHandler.CreateToken(tokenDescriptor);
            tokenObject.SigningKey = signature.Key;

            // Exract token
            var token = tokenHandler.WriteToken(tokenObject);

            return token;
        }

        private ClaimsIdentity GetClaims(string content)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _tokenSettings.Claims.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _tokenSettings.Claims.Audience),
                new Claim(JwtRegisteredClaimNames.Sub, _tokenSettings.Claims.Subject),
                new Claim(JwtRegisteredClaimNames.Exp,
                    DateTime.UtcNow
                    .AddMonths(_tokenSettings.Claims.Expires.Months)
                    .AddDays(_tokenSettings.Claims.Expires.Days)
                    .AddHours(_tokenSettings.Claims.Expires.Hours)
                    .ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(_tokenSettings.Claims.Content, content)
            };

            var payload = new ClaimsIdentity(claims);

            return payload;
        }

        private SecurityKey GetKey()
        {
            var secret = _tokenSettings.Secret;
            var bytes = Encoding.UTF8.GetBytes(secret);
            // var encodedSecret = Convert.ToBase64String(bytes);
            return new SymmetricSecurityKey(bytes);
        }

        private SigningCredentials GetSignature()
        {
            var key = GetKey();
            //SecurityAlgorithms.HmacSha256
            var secret = new SigningCredentials(key, _tokenSettings.Headers.Algorithm);
            return secret;
        }
}
    }
