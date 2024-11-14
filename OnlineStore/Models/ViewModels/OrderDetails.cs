namespace OnlineStore.Models.ViewModels
{
    public class OrderDetails
    {

        //The order itself
        public OrderDto Order { get; set; }

        //The the items associated to the order
        public IEnumerable<OrderItemDto> Items { get; set; }
    }
}
