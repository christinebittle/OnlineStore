namespace OnlineStore.Models.ViewModels
{
    public class OrderItemEdit
    {
        // Order item to edit
        public required OrderItemDto OrderItem { get; set; }

        //choose which product the order item refers
        public required IEnumerable<ProductDto> ProductOptions { get; set; }

        //choose which order the order item refers
        public required IEnumerable<OrderDto> OrderOptions { get; set; }
    }
}
