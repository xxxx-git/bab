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

        [AllowAnonymous]
        [HttpPost("generateToken")]
        public IActionResult GenerateToken([FromBody]AuthorizedUser userParam)
        {
            var token = _tokenService.GenerateToken(userParam);

            if (token == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(token);
        }

        [Authorize]
        [HttpGet("a")]
        public IActionResult value()
        {
            return Ok("pdony");
        }
    }

    public class AuthorizedUser : IUser
    {
        public string Id { get;set; }
        public string DisplayName { get; set;  }
        public string Hierarchy { get; set; }
        public string Token { get; set; }
    }
}