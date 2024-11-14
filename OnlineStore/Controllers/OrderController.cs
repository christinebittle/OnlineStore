using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        // dependency injection of service interfaces
        public OrderController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        /// <summary>
        /// Returns a list of Orders, each represented by an OrderDto with their associated Customer. Admin only
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderDto},{OrderDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Order/List
        /// HEADER: Cookie: .AspNetCore.Identity.Application={Token}
        /// -> [{OrderDto},{OrderDto},..]
        /// </example>
        [HttpGet(template: "List")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ListOrders()
        {
            // returns a list of order item dtos
            IEnumerable<OrderDto> OrderDtos = await _OrderService.ListOrders();
            // return 200 OK with OrderDtos
            return Ok(OrderDtos);
        }

        /// <summary>
        /// Returns a list of Orders for a customer, each represented by an OrderDto with their associated Customer
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderDto},{OrderDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Order/ListForCustomer/aaa-123 
        /// HEADERS: Cookie: .AspNetCore.Identity.Application={token}
        /// -> [{OrderDto},{OrderDto},..]
        /// </example>
        [HttpGet(template: "ListForCustomer/{customerId}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ListOrdersForCustomer(string customerId)
        {
            // returns a list of order dtos
            IEnumerable<OrderDto> OrderDtos = await _OrderService.ListOrdersForCustomer(customerId);
            // return 200 OK with OrderDtos
            return Ok(OrderDtos);
        }


        /// <summary>
        /// Returns a list of Orders for an account, each represented by an OrderDto with their associated Customer
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{OrderDto},{OrderDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Order/ListMyOrders
        /// HEADERS: Cookie: .AspNetCore.Identity.Application={token}
        ///-> [{OrderDto},{OrderDto},..]
        /// </example>
        [HttpGet(template: "ListMyOrders")]
        [Authorize(Roles = "admin,customer")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ListMyOrders()
        {
            // returns a list of order dtos
            IEnumerable<OrderDto> OrderDtos = await _OrderService.ListMyOrders();
            // return 200 OK with OrderDtos
            return Ok(OrderDtos);
        }
    }
}
