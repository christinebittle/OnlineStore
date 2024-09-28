using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface IOrderService
    {
        // ListOrders
        IEnumerable<OrderDto> ListOrders();

        // FindOrder(int id)

        // Add Order (order dto)

        // Update Order (order dto)

        // Delete Order (id)

        // related methods:

        // ListOrdersForCustomer(int customerid)
        IEnumerable<OrderDto> ListOrdersForCustomer(int customerId);
    }
}
