using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Data;
using OnlineStore.Models;

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

        public IEnumerable<OrderDto> ListOrders()
        {
            //todo: put into order Dto objects
            return new List<OrderDto>();
        }

        public IEnumerable<OrderDto> ListOrdersForCustomer(int customerId)
        {
            //todo: access the database
            return new List<OrderDto>();
        }

    }
}
