namespace OnlineStore.Models
{
    public class OrderItem
    {

        public int OrderItemId { get; set; }

        //Unit Price at time of purchase
        public decimal OrderItemUnitPrice { get; set; }

        //The number of items ordered
        public int OrderItemQty { get; set; }


        //An order item belongs to one order
        public required virtual Order Order { get; set; }
        public int OrderId { get; set; }

        //An order item belongs to one product
        public required virtual Product Product { get; set; }
        public int ProductId { get; set; }

    }

    public class OrderItemDto
    {
        public int? OrderItemId { get; set; }
        public decimal OrderItemUnitPrice { get; set; }

        public int OrderItemQty { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }


        //synthesized from OrderItemUnitPrice * OrderItemQty
        public decimal? OrderItemSubtotal { get; set; }

        //flattened from OrderItem -> Product
        public string? ProductSKU { get; set; }

        //flattened from OrderItem -> Order
        public string? OrderDate { get; set; }

        //flattened from OrderItem -> Order -> Customer
        public string? CustomerName { get; set; }
    }
}
