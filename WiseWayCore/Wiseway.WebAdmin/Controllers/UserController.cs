using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WiseWay.Core;
using WiseWay.Services;

namespace Wiseway.WebAdmin.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult LogIn()
        {
            return View();
        }

        [TempData]
        public string SessionUserId { get; set; }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Authenticate(User model)
        {
            var loginUser = _userService.Authenticate(model);

            if (loginUser == null)
                return View(loginUser);

            SessionUserId = JsonConvert.SerializeObject(loginUser.Id);
            TempData.Keep("SessionUserId");
            return RedirectToAction("Index", "Home");
        }
    }
}