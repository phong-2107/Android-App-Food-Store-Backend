using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface ICanteenRepository
    {
        Task<IEnumerable<Canteen>> GetCanteensAsync();
        Task<Canteen> GetCanteenByIdAsync(int id);
        Task AddCanteenAsync(Canteen canteen);
        Task UpdateCanteenAsync(Canteen canteen);
        Task DeleteCanteenAsync(int id);
    }
}
