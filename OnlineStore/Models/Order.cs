using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace OnlineStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public Province OrderProvince { get; set; }
        public Decimal OrderTotal { get; set; }

        public Decimal OrderTax { get; set; }

        public required string OrderTaxDesc { get; set; }
        public enum Province { ON, QC, NS, NB, MB, BC, PE, SK, AB, NL }


        //Each order belongs to one customer
        
        public IdentityUser Customer { get; set; }

        //An order can have many items
        public ICollection<OrderItem>? OrderItems { get; set; }

    }


    public class OrderDto
    {
        public int? OrderId { get; set; }

        public string OrderDate { get; set; }

        public string CustomerName { get; set; }

        //todo: add more information for Orders

    }

}
