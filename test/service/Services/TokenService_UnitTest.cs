using Xunit;
using Services;
using System.IdentityModel.Tokens.Jwt;
using bab;
using Microsoft.IdentityModel.Tokens;
using Shared.Config;

namespace test
{
    public class TokenService_UnitTest
    {
        private ITokenSettings _defaultSettings;

        public TokenService_UnitTest() 
        {
            _defaultSettings = new Settings() {
                Headers = new HeaderSettings() {
                    Algorithm = SecurityAlgorithms.HmacSha256,
                    Type = "jwt"
                },
                Claims = new ClaimsSettings() {
                    Expires = new ExpiresClaimSettings() {
                        Months = 0,
                        Days = 0,
                        Hours = 1
                    },
                    Audience = "test",
                    Content = "test",
                    Issuer = "test",
                    Subject = "test"
                },
                Secret = "testtesttesttesttesttesttesttesttest",
                Http = new TokenHttpSettings() {
                    Cookie = "test",
                    Header = "test",
                    HttpOnlyAccessCookie = true
                }
            };
        }

        [Fact]
        public void Generate_ShouldGenerateValidJwtToken()
        {
            var service = new SynTokenService(_defaultSettings);

            var token = service.Generate("content");
            var handler = new JwtSecurityTokenHandler();
            var isValid = handler.CanReadToken(token);

            Assert.Equal(isValid, true);
            _defaultSettings.Claims.Issuer = null;
        }
    }
}
