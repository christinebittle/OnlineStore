using OnlineStore.Interfaces;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace CoreEntityFramework.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly AppDbContext _context;

        // dependency injection of database context
        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        // implementations of customer create, read, update, delete go here

        public IEnumerable<CustomerDto> ListCustomers()
        {
            CustomerDto Customer1 = new CustomerDto()
            {
                CustomerId = 100,
                CustomerName = "Sam",
                CustomerEmail = "sam@test.ca"
            };

            CustomerDto Customer2 = new CustomerDto()
            {
                CustomerId = 101,
                CustomerName = "Alex",
                CustomerEmail = "Alex@test.ca"
            };

            List<CustomerDto> Customers = new List<CustomerDto>();

            Customers.Add(Customer1);
            Customers.Add(Customer2);

            return Customers;

        }
    }
}
