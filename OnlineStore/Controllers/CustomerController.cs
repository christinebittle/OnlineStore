using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;

using Microsoft.AspNetCore.Authorization;
using CoreEntityFramework.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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


        /// <summary>
        /// Gets a list of customers in the system. Administrator only.
        /// </summary>
        /// <returns>
        /// List of type CustomerDto
        /// </returns>
        /// <example>
        /// GET: api/Customer/List -> [{CustomerDto},{CustomerDto}]
        //  HEADERS: Cookie: .AspNetCore.Identity.Application={token}
        /// </example>
        [HttpGet(template: "List")]
        [Authorize(Roles="admin")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> ListCustomers()
        {
            return Ok(await _customerService.ListCustomers());
        }


        /// <summary>
        /// Finds a customer in the system. Administrators can access any customer.
        /// </summary>
        /// <returns>
        /// CustomerDto
        /// </returns>
        /// <example>
        /// GET: api/Customer/Find/{id}
        //  HEADERS: Cookie: .AspNetCore.Identity.Application={token}
        // -> {CustomerDto}
        /// </example>
        [HttpGet(template: "Find/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CustomerDto>> FindCustomer(string id)
        {
            CustomerDto? Customer = await _customerService.FindCustomer(id);
            // if the Customer could not be located, return 404 Not Found
            if (Customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Customer);
            }
        }

        /// <summary>
        /// Gets a single customer's profile (based on their logged in information)
        /// </summary>
        /// <returns>
        /// CustomerDto
        /// </returns>
        /// <example>
        /// GET: api/Customer/Profile
        //  HEADERS: Cookie: .AspNetCore.Identity.Application={token}
        // -> {CustomerDto}
        /// </example>
        [HttpGet(template: "Profile")]
        [Authorize(Roles = "admin,customer")]
        public async Task<ActionResult<CustomerDto>> Profile()
        {
            
            CustomerDto? Customer = await _customerService.Profile();
            // if the Customer could not be located, return 404 Not Found
            if (Customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Customer);
            }
        }


    }
}
