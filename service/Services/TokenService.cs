using bab.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Config;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace bab.Services {
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;

        public TokenService(IOptions<JWTSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public IUser GenerateToken(IUser authrizedUser)
        {
            var header = new JwtHeader();
            header.Add("typ", "jwt");
            header.Add("alg", "HS256");
            
            var x = header.Alg;

            var payload = new JwtPayload();

            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, authrizedUser.Id),
                new Claim(JwtRegisteredClaimNames.Sub, authrizedUser.Hierarchy),
                new Claim(JwtRegisteredClaimNames.Sub, authrizedUser.DisplayName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bababababababaababbabababaab"));

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(1),
                claims: claims,
                signingCredentials: new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256)
            );
            return null;

            // authentication successful so generate jwt token
            // var tokenHandler = new JwtSecurityTokenHandler();
            // var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(new Claim[] 
            //     {
            //         new Claim(ClaimTypes.Name, authrizedUser.Id.ToString())
            //     }),
            //     Expires = DateTime.UtcNow.AddDays(7),
            //     SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            // };
            
            // var token = tokenHandler.CreateToken(tokenDescriptor);
            // authrizedUser.Token = tokenHandler.WriteToken(token);

            // return authrizedUser;
        }
    }
}