using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface IOrderItemService
    {
        // describes the ways we interact with OrderItemService
        // Very useful if "OrderItemService" is replaced with "OrderItemTester"
        Task<IEnumerable<OrderItemDto>> ListOrderItems();


        Task<OrderItemDto?> FindOrderItem(int id);


        Task<ServiceResponse> UpdateOrderItem(OrderItemDto orderItemDto);

        Task<ServiceResponse> AddOrderItem(OrderItemDto orderItemDto);

        Task<ServiceResponse> DeleteOrderItem(int id);

        // related methods

        Task<IEnumerable<OrderItemDto>> ListOrderItemsForOrder(int id);


        Task<IEnumerable<OrderItemDto>> ListOrderItemsForProduct(int id);

    }
}
