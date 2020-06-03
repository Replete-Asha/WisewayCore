using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wiseway.WebAdmin.Models;
using WiseWay.Core;
using WiseWay.Extender;
using WiseWay.Services;

namespace Wiseway.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Dashboard()
        {
            CustomerService _service = new CustomerService();
            string Jsonstring = _service.GetCustomerTypeWiseCount();
            DataTable dt = (DataTable)JsonConvert.DeserializeObject(Jsonstring, (typeof(DataTable)));
            long TotalCustomer = 0, TotalReSeller = 0, TotalWholeSeller = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["CustomerType"].ToString() == "Customer")
                    {
                        TotalCustomer = dr["cnt"].ToLong();
                    }
                    else if(dr["CustomerType"].ToString() == "ReSeller")
                    {
                        TotalReSeller = dr["cnt"].ToLong();
                    }
                    else if(dr["CustomerType"].ToString() == "WholeSeller")
                    {
                        TotalWholeSeller = dr["cnt"].ToLong();
                    }
                }
                TempData["TotalCustomer"] = TotalCustomer;
                TempData["TotalReSeller"] = TotalReSeller;
                TempData["TotalWholeSeller"] = TotalWholeSeller;
            }
            return View();
        }
    }
}
