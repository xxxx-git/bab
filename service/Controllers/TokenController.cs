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
            var token = _tokenService.Generate(userParam);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }

        [HttpGet("verify")]
        public IActionResult Verify()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            var user = _tokenService.Verify(token);
            return Ok(user);
        }

        [HttpGet("decode")]
        public IActionResult value()
        {
            var y = HttpContext.Request;
            return Ok("pdony");
        }
    }
}