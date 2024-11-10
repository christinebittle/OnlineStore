using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface IOrderService
    {
        // ListOrders
        Task<IEnumerable<OrderDto>> ListOrders();

        // FindOrder(int id)

        // Add Order (order dto)

        // Update Order (order dto)

        // Delete Order (id)

        // related methods:

        // ListOrdersForCustomer(int customerid)
        Task<IEnumerable<OrderDto>> ListOrdersForCustomer(string customerId);

        Task<IEnumerable<OrderDto>> ListMyOrders();

        Task<OrderDto> FindOrder(int orderId);

    }
}
