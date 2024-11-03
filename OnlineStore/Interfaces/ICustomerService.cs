using OnlineStore.Models;

namespace OnlineStore.Interfaces
{
    public interface ICustomerService
    {
        // definitions for implementations of actions to create, read, update, delete

        // base CRUD
        Task<IEnumerable<CustomerDto>> ListCustomers();

        Task<CustomerDto?> FindCustomer(string id);

        Task<CustomerDto?> Profile();

        // Most CRUD is executed by the user on their account

        // Administrator CRUD on customers (users)

        // DeleteUser
        
        // related methods

        
    }
}
