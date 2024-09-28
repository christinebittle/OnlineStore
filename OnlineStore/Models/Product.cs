using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public required string ProductSKU { get; set; }

        public decimal ProductPrice { get; set; }

        //A product can have many categories
        public ICollection<Category>? Categories { get; set; }

        //A product can be a part of many ordered items
        public ICollection<OrderItem>? Items { get; set; }

    }

    public class ProductDto
    {
        //todo: fill out Data transfer object
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductSKU { get; set; }

        public decimal ProductPrice { get; set; }
    }
}
