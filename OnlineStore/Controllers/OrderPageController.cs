using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class OrderPageController : Controller
    {
        private readonly IOrderService _orderService;

        // dependency injection of service interfaces
        public OrderPageController(IOrderService OrderService)
        {
            _orderService = OrderService;
        }

        [Authorize(Roles="customer,admin")]
        public async Task<IActionResult> List()
        {
            IEnumerable<OrderDto> MyOrders = await _orderService.ListMyOrders();
            return View(MyOrders);
        }

        //todo : show order (admin), show order (customer), update delete add order
    }
}
