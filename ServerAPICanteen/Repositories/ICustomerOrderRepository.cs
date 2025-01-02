using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface ICustomerOrderRepository
    {
        Task<CustomerOrder> GetCustomerOrderByIdAsync(int id);
        Task CreateCustomerOrderAsync(CustomerOrder customerOrder);
        Task UpdateCustomerOrderAsync(CustomerOrder customerOrder);
        Task DeleteCustomerOrderAsync(int id);
    }
}
