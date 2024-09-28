using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;

namespace CoreEntityFramework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        // dependency injection of service interfaces
        public CustomerController(ICustomerService CustomerService)
        {
            _customerService = CustomerService;
        }

        //get: api/customer/list -> [{Customer},{CustomerDto}]
        [HttpGet(template: "List")]
        public IEnumerable<CustomerDto> ListCustomers()
        {
            return _customerService.ListCustomers();

        }


    }
}
