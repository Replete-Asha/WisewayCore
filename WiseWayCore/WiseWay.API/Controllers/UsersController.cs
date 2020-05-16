using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiseWay.Core;
using WiseWay.Services;

namespace WiseWay.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User model)
        {
            var users = _userService.Authenticate(model);

            if (users == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("AddUpdateUser")]
        public IActionResult AddUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {    
                var users = _userService.AddUpdateUser(model);
                return Ok(users);
            }
            return BadRequest();
        }
    }
}