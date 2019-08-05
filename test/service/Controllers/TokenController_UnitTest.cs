using Xunit;
using Services;
using System.IdentityModel.Tokens.Jwt;
using bab.Controllers;
using Microsoft.IdentityModel.Tokens;
using Shared.Config;
using System.Collections.Generic;
using Shared;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace test
{
    public class TokenController_UnitTest
    {
        private readonly TokenController _tokenController;
        private readonly string FAKE_PAYLOD_AS_STR = "{ \"Key\": \"Value\"}";
        private readonly string FAKE_TOKEN = "1234faketoken5678";
        private readonly string COOKIE_HEADER = "Authorization";
        
        public TokenController_UnitTest() 
        {
            var mockTokenService = new Mock<ISecurityTokenService>();
            mockTokenService.Setup(m => m.Generate(
                It.IsAny<string>()
            )).Returns(FAKE_TOKEN);
             mockTokenService.Setup(m => m.Verify(
                It.IsAny<string>()
            )).Returns(FAKE_PAYLOD_AS_STR);

            var mockJsonService = new Mock<IJsonCovnertService>();
            mockJsonService.Setup(m => m.Serialize(
                It.IsAny<object>()
            )).Returns(FAKE_PAYLOD_AS_STR);;

            var mockTokenHttpSettings = new Mock<ITokenHttpSettings>();
            mockTokenHttpSettings.SetupSet(m => m.Header =  It.IsAny<string>());
            mockTokenHttpSettings.SetupGet(m => m.Header).Returns(COOKIE_HEADER);
            mockTokenHttpSettings.SetupSet(m => m.Cookie =  It.IsAny<string>());
            mockTokenHttpSettings.SetupGet(m => m.Cookie).Returns(COOKIE_HEADER);
            mockTokenHttpSettings.SetupSet(m => m.HttpOnlyAccessCookie =  It.IsAny<bool>());
            mockTokenHttpSettings.SetupGet(m => m.HttpOnlyAccessCookie).Returns(true);

            _tokenController = new TokenController(mockTokenService.Object, mockJsonService.Object, mockTokenHttpSettings.Object);
        }

        [Fact]
        public void GenerateToken_ReturnsOkResult()
        {
            //Arrange
            var json = FAKE_PAYLOD_AS_STR;
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()));
            _tokenController.ControllerContext =  new ControllerContext {
                HttpContext = mockHttpContext.Object
            };
            
            // Act
            IActionResult actualResponse = _tokenController.GenerateToken(json);
            var x=  _tokenController.Response.Headers;
            
            // Assert
            Assert.IsType<OkObjectResult>(actualResponse);
            Assert.Equal(FAKE_TOKEN, (actualResponse as OkObjectResult).Value);
            mockHttpContext.Verify(m => m.Response.Cookies.Append(It.Is<string>(s => s == COOKIE_HEADER), It.IsNotNull<string>(), It.IsAny<CookieOptions>()));
        }

        [Fact]
        public void ValidateToken_ReturnsOkResult()
        {
            //Arrange
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(m => m.Request.Cookies[It.IsAny<string>()]).Returns(FAKE_TOKEN);
           
            _tokenController.ControllerContext =  new ControllerContext {
                HttpContext = mockHttpContext.Object
            };
            
            // Act
            var actualResponse = _tokenController.Verify();

            // Assert
            Assert.IsType<OkObjectResult>(actualResponse);
            Assert.Equal(FAKE_PAYLOD_AS_STR, (actualResponse as OkObjectResult).Value);
        }
    }
}
