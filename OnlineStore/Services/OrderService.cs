using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Data;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreEntityFramework.Services
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;

        // dependency injection of database context
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> ListOrders()
        {
            // all categories
            List<Order> Orders = await _context.Orders
                .Include(o=>o.Customer)
                .ToListAsync();
            // empty list of data transfer object CategoryDto
            List<OrderDto> OrderDtos = new List<OrderDto>();
            // foreach Order Item record in database
            foreach (Order Order in Orders)
            {
                // create new instance of CategoryDto, add to list
                OrderDtos.Add(new OrderDto()
                {
                    OrderId = Order.OrderId,
                    OrderDate = Order.OrderDate.ToString("yyyy-MM-dd"),
                    CustomerName = Order.Customer.CustomerName
                });
            }
            // return CategoryDtos
            return OrderDtos;
        }

        public IEnumerable<OrderDto> ListOrdersForCustomer(int customerId)
        {
            //todo: access the database
            return new List<OrderDto>();
        }

    }
}
