using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    
    // no model, we are bootstrapping the Identity User as the Customer

    public class CustomerDto
    {
        public required string CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerEmail { get; set; }

    }
}
