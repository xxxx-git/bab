using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared;
using Shared.Config;
using Microsoft.AspNetCore.Cors;

namespace bab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private ISecurityTokenService _tokenService;
        private IJsonCovnertService _jsonService;
        private ITokenHttpSettings _tokenHttpSettings;
        public TokenController(ISecurityTokenService tokenService,
            IJsonCovnertService jsonService, ITokenHttpSettings tokenHttpSettings)
        {
            _tokenService = tokenService;
            _jsonService = jsonService;
            _tokenHttpSettings = tokenHttpSettings;
        }

        [EnableCors("AllowCorsPolicy")]
        [HttpPost("generate")]
        public IActionResult GenerateToken([FromBody]dynamic json)
        {
            var stringifyContent = _jsonService.Serialize(json);
            var token = _tokenService.Generate(stringifyContent);

            if (token == null)
                return BadRequest(new { message = "Could not generate token with given content" });
            
            var options = new CookieOptions();
            options.HttpOnly = _tokenHttpSettings.HttpOnlyAccessCookie;
            HttpContext.Response.Cookies.Append(_tokenHttpSettings.Cookie, token, options);
            return Ok(token);
        }

        [EnableCors("AllowCorsPolicy")]
        [HttpGet("verify")]
        public IActionResult Verify()
        {
            var token = HttpContext.Request.Cookies[_tokenHttpSettings.Header];
            var stringfyContent = _tokenService.Verify(token);
            return Ok(stringfyContent);
        }
    }
}