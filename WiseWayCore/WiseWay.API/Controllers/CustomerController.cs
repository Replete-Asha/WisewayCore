using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseWay.Core;
using WiseWay.Services;

namespace WiseWay.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService CustomerService)
        {
            _customerService = CustomerService;
        }

        [HttpPost("AddUpdateCustomer")]
        public IActionResult AddCustomer([FromBody]Customer model)
        {
            if (ModelState.IsValid)
            {
                var Customers = _customerService.AddUpdateCustomer(model);
                return Ok(Customers);

            }
            return BadRequest();
        }

        [HttpGet("GetCustomerList")]
        public string GetCustomerList()
        {
            string result = _customerService.GetCustomerList();
            if (string.IsNullOrEmpty(result))
            {
                return "{\"msg\":\"No data \"}";
            }
            return result;
        }

        [HttpGet("DeleteCustomer/{CustomerId}")]
        public string DeleteCustomer(int CustomerId)
        {
            string result = _customerService.DeleteCustomer(CustomerId);
            return result;
        }

        [HttpGet("GetCustomerDetailById/{CustomerId}")]
        public Customer GetCustomerDetailById(int CustomerId)
        {
            return _customerService.GetCustomerDetailById(CustomerId);
        }

        [HttpGet("GetCustomerTypeWiseCount")]
        public string GetCustomerTypeWiseCount()
        {
            string result = _customerService.GetCustomerTypeWiseCount();
            if (string.IsNullOrEmpty(result))
            {
                return "{\"msg\":\"No data \"}";
            }
            return result;
        }
    }
}