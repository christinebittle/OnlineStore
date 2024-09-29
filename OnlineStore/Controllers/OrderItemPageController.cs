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
        

        // dependency injection of service interface
        public OrderItemPageController(IOrderItemService OrderItemService)
        {

            _orderItemService = OrderItemService;
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

        // GET OrderItemPage/New
        public ActionResult New()
        {
            return View();
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

        //GET OrderItemPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

        //POST OrderItemPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, OrderItemDto OrderItemDto)
        {
            ServiceResponse response = await _orderItemService.UpdateOrderItem(OrderItemDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "OrderItemPage", new { id = id });
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
    }
}
