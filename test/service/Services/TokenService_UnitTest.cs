using Xunit;
using Services;
using System.IdentityModel.Tokens.Jwt;
using bab;
using Microsoft.IdentityModel.Tokens;
using Shared.Config;
using System.Collections.Generic;

namespace test
{
    public class TokenService_UnitTest
    {
        private static ITokenSettings defaultSettings = new Settings() {
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
        private readonly SynTokenService _service;

        public TokenService_UnitTest() 
        {
            _service = new SynTokenService(defaultSettings);
        }

        [Theory]
        [InlineData("content", true)]
        [InlineData("", true)]
        [InlineData(null, true)]
        [InlineData("{\"content\":1}", true)]
        public void Generate_ShouldGenerateValidJwtToken(string content, bool expected)
        {
            var actualToken = _service.Generate(content);

            var handler = new JwtSecurityTokenHandler();
            Assert.Equal(handler.CanReadToken(actualToken), expected);
        }

        [Theory]
        [MemberData(nameof(getVerifyTestInputs), parameters: "content")]
        [MemberData(nameof(getVerifyTestInputs), parameters: "{\"content\": \"bla\"}")]
        [InlineData("" ,null)]
        [InlineData("bla",null)]
        [InlineData("bla.bla.bla",null)]
        [InlineData("...",null)]
        [InlineData(null,null)]
        public void Verify_ShouldReturnContentIfTokenValid(string token, string expected)
        {
            string actualContent = _service.Verify(token);

            Assert.Equal(expected, actualContent);
        }

        public static IEnumerable<object[]> getVerifyTestInputs(string content)
        {
            SynTokenService service = new SynTokenService(defaultSettings);
            var allTestInputs =  new List<object[]>
            {
                new object [] {service.Generate(content), content},
            };
            return allTestInputs;
        }
    }
}
