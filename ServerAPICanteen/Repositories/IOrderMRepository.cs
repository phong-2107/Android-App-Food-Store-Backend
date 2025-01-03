using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface IOrderMRepository
    {
        Task<IEnumerable<CustomerOrder>> GetAllAsync();
        Task<CustomerOrder?> GetByIdAsync(int id);
    }
}
