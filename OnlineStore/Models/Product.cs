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

        // flag for content that is ai generated
        public bool ai_generated { get; set; }
        public string ProductDescription { get; set; }
        public bool HasPic { get; set; } = false;
        
        // images stored in /wwwroot/images/products/{ProductId}.{PicExtension}
        public string? PicExtension { get; set; }



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

        public bool ai_generated { get; set; }

        public string ProductDescription { get; set; }

        // bool if there is a pic, false otherwise
        public bool HasProductPic { get; set; }

        // string to represent the path to the product picture
        public string? ProductImagePath { get; set; }
    }
}
