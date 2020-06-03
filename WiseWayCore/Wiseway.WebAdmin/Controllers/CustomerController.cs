using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WiseWay.Core;
using WiseWay.Services;

namespace Wiseway.WebAdmin.Controllers
{
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService CustomerService)
        {
            _customerService = CustomerService;
        }
        public IActionResult Index()
        {
            List<Customer> lst = new List<Customer>();
            lst = _customerService.GetCustomers();
            return View(lst);
        }
        public ActionResult Create()
        {
            TempData.Keep("SessionUserId");
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind] Customer objModel)
        {
            if (ModelState.IsValid)
            {
                objModel.UserId = Convert.ToInt32(TempData["SessionUserId"]);
                _customerService.AddUpdateCustomer(objModel);
                TempData["Success"] = "Information added successfully!";
                return RedirectToAction("Index");
            }
            return View(objModel);
        }

        public ActionResult Edit(int Id)
        {
            TempData.Keep("SessionUserId");
            Customer objModel = _customerService.GetCustomerDetailById(Id);
            if (objModel == null)
            {
                return NotFound();
            }
            return View(objModel);
        }

        [HttpPost]
        public IActionResult Edit([Bind] Customer objModel)
        {
            if (ModelState.IsValid)
            {
                objModel.UserId = Convert.ToInt32(TempData["SessionUserId"]);
                _customerService.AddUpdateCustomer(objModel);
                TempData["Success"] = "Information updated successfully!";
                return RedirectToAction("Index");
            }
            return View(objModel);
        }
        public ActionResult Delete(int Id)
        {
            _customerService.DeleteCustomer(Id);
            ViewBag.Message = "Delete";
            TempData["Success"] = "Information deleted successfully!";
            return RedirectToAction("Index");
        }        
    }
}