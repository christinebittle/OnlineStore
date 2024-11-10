using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CoreEntityFramework.Services
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // dependency injection of database context
        public OrderService(AppDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IEnumerable<OrderDto>> ListOrders()
        {
            // all orders
            List<Order> Orders = await _context.Orders
                .Include(o=>o.Customer)
                .Include(o=>o.OrderItems)
                .ToListAsync();
            // empty list of data transfer object OrderDto
            List<OrderDto> OrderDtos = new List<OrderDto>();
            // foreach Order record in database
            foreach (Order Order in Orders)
            {
                // create new instance of OrderDto, add to list
                OrderDtos.Add(new OrderDto()
                {
                    OrderId = Order.OrderId,
                    OrderDate = Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = Order.Customer.UserName,
                    CustomerId = Order.Customer.Id,
                    NumItems = Order.OrderItems.Count()
                });
            }
            // return OrderDtos
            return OrderDtos;
        }

        public async Task<IEnumerable<OrderDto>> ListOrdersForCustomer(string customerId)
        {
            // all orders for a customer
            List<Order> Orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.Customer.Id==customerId)
                .ToListAsync();
            // empty list of data transfer object OrderDto
            List<OrderDto> OrderDtos = new List<OrderDto>();
            // foreach Order record in database
            foreach (Order Order in Orders)
            {
                // create new instance of OrderDto, add to list
                OrderDtos.Add(new OrderDto()
                {
                    OrderId = Order.OrderId,
                    OrderDate = Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = Order.Customer.UserName,
                    CustomerId = Order.Customer.Id,
                    NumItems = Order.OrderItems.Count()
                });
            }
            // return OrderDtos
            return OrderDtos;
        }


        public async Task<IEnumerable<OrderDto>> ListMyOrders()
        {
            IdentityUser? User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string customerId = User.Id;

            // all orders for a customer
            List<Order> Orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .Where(o => o.Customer.Id == customerId)
                .ToListAsync();
            // empty list of data transfer object OrderDto
            List<OrderDto> OrderDtos = new List<OrderDto>();
            // foreach Order record in database
            foreach (Order Order in Orders)
            {
                // create new instance of OrderDto, add to list
                OrderDtos.Add(new OrderDto()
                {
                    OrderId = Order.OrderId,
                    OrderDate = Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = Order.Customer.UserName,
                    CustomerId = Order.Customer.Id,
                    NumItems = Order.OrderItems.Count()
                });
            }
            // return OrderDtos
            return OrderDtos;
        }

    }
}
