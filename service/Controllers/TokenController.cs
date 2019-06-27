using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bab.Shared;

namespace bab.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        
        [HttpPost("generateToken")]
        public IActionResult GenerateToken([FromBody]AuthorizedUser userParam)
        {
            var user = _tokenService.GenerateToken(userParam);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // [HttpGet]
        // public IActionResult GetAll()
        // {
        //     var users =  _userService.GetAll();
        //     return Ok(users);
        // }
    }

    public class AuthorizedUser : IUser
    {
        public string Id { get;set; }
        public string DisplayName { get; set;  }
        public string Hierarchy { get; set; }
        public string Token { get; set; }
    }
}