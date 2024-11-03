using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Review
    {

        [Key]
        public int ReviewId { get; set; }

        public decimal ReviewRating { get; set; }

        public required string ReviewContent { get; set; }

        public DateTime ReviewDate { get; set; }

        // A review belongs to a product-order pair
        // implictly belongs to a customer (OrderItem > Order > Customer)
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }

    }
}
