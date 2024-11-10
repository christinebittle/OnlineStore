using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class OrderPageController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        // dependency injection of service interfaces
        public OrderPageController(IOrderService OrderService, IOrderItemService OrderItemService)
        {
            _orderService = OrderService;
            _orderItemService = OrderItemService;
        }

        [Authorize(Roles="customer,admin")]
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            IEnumerable<OrderDto> MyOrders = await _orderService.ListMyOrders();
            return View(MyOrders);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<OrderDto> MyOrders = await _orderService.ListOrders();
            return View(MyOrders);
        }

        [Authorize(Roles = "customer,admin")]
        [HttpGet]
        public async Task<IActionResult> Show(int id)
        {
            OrderDto SelectedOrder = await _orderService.FindOrder(id);
            IEnumerable<OrderItemDto> Items = await _orderItemService.ListOrderItemsForOrder(id);

            OrderDetails ViewModel = new OrderDetails();
            ViewModel.Order = SelectedOrder;
            ViewModel.Items = Items;

            return View(ViewModel);
        }

        //todo : show order (admin), show order (customer), update delete add order
    }
}
