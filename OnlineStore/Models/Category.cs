using OnlineStore.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public required string CategoryName { get; set; }

        public required string CategoryDescription { get; set; }

        public required string CategoryColor { get; set; }

        //A category can be applied to multiple products
        public ICollection<Product>? Products { get; set; }

    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public required string CategoryName { get; set; }

        public required string CategoryDescription { get; set; }

        public required string CategoryColor { get; set; }
    }
}