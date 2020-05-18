using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("AddUpdateUser")]
        public IActionResult AddUser([FromBody]User model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.PhoneNo) || string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
                {
                    return NotFound(new { message = "Phone No,First Name,Last Name are compulsory fields" });
                }
                var users = _userService.AddUpdateUser(model);
                return Ok(users);

            }
            return BadRequest();
        }

        [HttpGet("GetUserList")]
        public string GetUserList()
        {
            string result = _userService.GetUserList();
            if (string.IsNullOrEmpty(result))
            {
                return "{\"msg\":\"No data \"}";
            }
            return result;
        }

        [HttpGet("DeleteUser/{UserId}")]
        public string DeleteUser(int UserId)
        {
            string result = _userService.DeleteUser(UserId);
            return result;
        }

        [HttpGet("ChangeUserStatus/{UserId}/{IsActive}")]
        public string ChangeUserStatus(int UserId, bool IsActive)
        {
            string result = _userService.ChangeUserStatus(UserId, IsActive);
            return result;
        }

        [HttpGet("GetUserDetailById/{UserId}")]
        public User GetUserDetailById(int UserId)
        {
            return _userService.GetUserDetailById(UserId);
        }
    }
}