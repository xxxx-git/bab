using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared;

namespace bab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private ISecurityTokenService _tokenService;
        public TokenController(ISecurityTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("generate")]
        public IActionResult GenerateToken([FromBody]dynamic userParam)
        {
            var stringifyUser = Newtonsoft.Json.JsonConvert.SerializeObject(userParam);
            var token = _tokenService.Generate(stringifyUser);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            var options = new CookieOptions();
            options.HttpOnly = true;
            // options.Expires = DateTime.UtcNow.
            HttpContext.Response.Cookies.Append("auth", token, options);
            return Ok(token);
        }

        [HttpGet("verify")]
        public IActionResult Verify()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var user = _tokenService.Verify(token);
            var jsonUser = Newtonsoft.Json.JsonConvert.DeserializeObject(user);
            return Ok(jsonUser);
        }

        [HttpGet("decode")]
        public IActionResult value()
        {
            var y = HttpContext.Request;
            return Ok("pdony");
        }
    }
}