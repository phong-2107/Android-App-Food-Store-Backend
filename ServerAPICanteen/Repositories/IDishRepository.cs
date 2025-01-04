using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
        Task UpdateDishPictureAsync(int id, string pictureUrl);
        Task<IEnumerable<object>> GetDishesWithCategoryAsync();
    }
}
