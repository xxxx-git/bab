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
        private readonly ITokenSettings _defaultSettings;
        private readonly string _content, _validToken, _invalidToken;

        private readonly SynTokenService _service;

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

            _service = new SynTokenService(_defaultSettings);

            _content = "content";

            _validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0ZXN0IiwiYXVkIjoidGVzdCIsInN1YiI6InRlc3QiLCJleHAiOjE1NjMxMTg0ODAsIm5iZiI6MTU2MzExNDg4MCwianRpIjoiZDhiZDg5NjYtMGQyZi00M2QzLTgwN2ItMjE2ZDVhYWIwYzg0IiwidGVzdCI6ImNvbnRlbnQiLCJpYXQiOjE1NjMxMTQ4ODB9.TPqXZR-L9GzPdybt5fwvlVqt9l0PhB6I98PNZugrDG4";
            _invalidToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0ZXN0IiwiYXVkIjoidGVzdCIsInN1YiI6InRlc3QiLCJleHAiOjE1NjMxMTg0ODAsIm5iZiI6MTU2MzExNDg4MCwianRpIjoiZDhiZDg5NjYtMGQyZi00M2QzLTgwN2ItMjE2ZDVhYWIwYzg0IiwidGVzdCI6ImNvbnRlbnQiLCJpYXQiOjE1NjMxMTQ4ODB9.TPqXZR-L9GzPdybt5fwvlVqt9l0PhB6I98PNZug5555";
        }

        [Fact]
        public void Generate_ShouldGenerateValidJwtToken()
        {
            var actualToken = _service.Generate(_content);

            var handler = new JwtSecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(actualToken), true);
        }

        [Fact]
        public void Verify_ShouldReturnContentOnValidToken()
        {
            string actualContent = _service.Verify(_validToken);

            Assert.Equal(_content, actualContent);
        }

        [Fact]
        public void Verify_ShouldFailOnInValidToken()
        {
            string actualContent = _service.Verify(_invalidToken);

            Assert.Equal(null, actualContent);
        }
    }
}
