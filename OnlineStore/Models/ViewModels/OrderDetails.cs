namespace OnlineStore.Models.ViewModels
{
    public class OrderDetails
    {

        public OrderDto Order { get; set; }

        public IEnumerable<OrderItemDto> Items { get; set; }
    }
}
