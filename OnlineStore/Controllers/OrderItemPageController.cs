using Microsoft.AspNetCore.Mvc;
using OnlineStore.Interfaces;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;
using OnlineStore.Services;

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

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: OrderItemPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<OrderItemDto?> OrderItemDtos = await _orderItemService.ListOrderItems();
            return View(OrderItemDtos);
        }

        //GET OrderItemPage/Edit/{id}
        [HttpGet]
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

        /*
        // GET: OrderItemPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            OrderItemDto? OrderItemDto = await _orderItemService.FindOrderItem(id);
           

            if (OrderItemDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find OrderItem"] });
            }
            else
            {
                return View(OrderItemDto);
            }
        }

        


        // POST OrderItemPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(OrderItemDto OrderItemDto)
        {
            ServiceResponse response = await _orderItemService.AddOrderItem(OrderItemDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "OrderItemPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        

        

        //GET OrderItemPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            OrderItemDto? OrderItemDto = await _orderItemService.FindOrderItem(id);
            if (OrderItemDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(OrderItemDto);
            }
        }

        //POST OrderItemPage/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _orderItemService.DeleteOrderItem(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "OrderItemPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }


        */
    }
}
