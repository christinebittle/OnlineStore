﻿using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;
using OnlineStore.Services;
using Microsoft.AspNetCore.Authorization;

namespace OnlineStore.Controllers
{
    public class OrderItemPageController : Controller
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        // dependency injection of service interface
        public OrderItemPageController(IOrderItemService OrderItemService, IProductService ProductService, IOrderService OrderService)
        {
            _productService = ProductService;
            _orderItemService = OrderItemService;
            _orderService = OrderService;
        }

        // GET: OrderItemPage
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: OrderItemPage/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<OrderItemDto?> OrderItemDtos = await _orderItemService.ListOrderItems();
            return View(OrderItemDtos);
        }

        //GET OrderItemPage/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            OrderItemDto? OrderItemDto = await _orderItemService.FindOrderItem(id);
            IEnumerable<ProductDto> Products = await _productService.ListProducts();
            IEnumerable<OrderDto> Orders = await _orderService.ListOrders();
            if (OrderItemDto == null)
            {
                return View("Error");
            }
            else
            {
                OrderItemEdit OrderItemInfo = new OrderItemEdit()
                {
                    OrderItem = OrderItemDto,
                    ProductOptions = Products,
                    OrderOptions = Orders
                }; 
                return View(OrderItemInfo);

            }
        }

        //POST OrderItemPage/Update/{id}
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(int id, OrderItemDto OrderItemDto)
        {
            ServiceResponse response = await _orderItemService.UpdateOrderItem(OrderItemDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("List", "OrderItemPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET OrderItemPage/New
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> New()
        {

            IEnumerable<ProductDto?> ProductDtos = await _productService.ListProducts();

            IEnumerable<OrderDto?> OrderDtos = await _orderService.ListOrders();

            OrderItemNew Options = new OrderItemNew() { 
                AllOrders = OrderDtos,
                AllProducts = ProductDtos
            };



            return View(Options);
        }

        // POST OrderItemPage/Add
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(OrderItemDto OrderItemDto)
        {
            ServiceResponse response = await _orderItemService.AddOrderItem(OrderItemDto);

            //checking if the item was added
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                //return RedirectToAction("Details", "OrderItemPage",new { id=response.CreatedId });
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }

            

        }

        
    }
}
