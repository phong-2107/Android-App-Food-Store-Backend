using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public interface IDishRepository
    {
        Task<IEnumerable<Dish>> GetDishesAsync();
        Task<Dish> GetDishByIdAsync(int id);
        Task<IEnumerable<Dish>> GetDishesOrderedByDishRatingAsync();
        Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
    }
}
