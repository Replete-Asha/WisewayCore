using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
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
            return RedirectToAction("Dashboard", "Home");
        }
        public ActionResult Create()
        {
            TempData.Keep("SessionUserId");
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind] User objModel)
        {
            if (ModelState.IsValid)
            {
                var UserId = TempData["SessionUserId"];
                _userService.AddUpdateUser(objModel);
                TempData["Success"] = "Information added successfully!";
                return RedirectToAction("Index");
            }
            return View(objModel);
        }

        public ActionResult Edit(int Id)
        {
            TempData.Keep("SessionUserId");
            User objModel = _userService.GetUserDetailById(Id);
            if (objModel == null)
            {
                return NotFound();
            }
            return View(objModel);
        }

        [HttpPost]
        public IActionResult Edit([Bind] User objModel)
        {
            if (ModelState.IsValid)
            {
                _userService.AddUpdateUser(objModel);
                TempData["Success"] = "Information updated successfully!";
                return RedirectToAction("Index");
            }
            return View(objModel);
        }
        public ActionResult Delete(int Id)
        {
            _userService.DeleteUser(Id);
            ViewBag.Message = "Delete";
            TempData["Success"] = "Information deleted successfully!";
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            List<User> lst = new List<User>();
            lst = _userService.GetUsers();
            return View(lst);
        }
    }
}