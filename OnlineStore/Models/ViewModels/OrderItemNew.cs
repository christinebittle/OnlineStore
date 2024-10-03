namespace OnlineStore.Models.ViewModels
{
    public class OrderItemNew
    {
        //the ViewModel I need to package the options for the new Order Item

        // For a list of products to choose from
        public IEnumerable<ProductDto> AllProducts { get; set; }

        // For a list of orders to choose from
        public IEnumerable<OrderDto> AllOrders { get; set; }
    }
}
