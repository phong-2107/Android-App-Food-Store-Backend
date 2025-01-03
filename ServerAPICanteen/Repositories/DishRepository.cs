using Microsoft.EntityFrameworkCore;
using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly ApplicationDbContext _context;

        public DishRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dish>> GetDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }

        public async Task<IEnumerable<Dish>> GetDishesOrderedByDishRatingAsync()
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .OrderByDescending(d => d.Rating)
                .Select(d => new Dish
                {
                    IdDish = d.IdDish,
                    DishName = d.DishName,
                    PictureUrlArray = d.PictureUrlArray,
                    Amount = d.Amount,
                    DishStats = d.DishStats,
                    Description = d.Description,
                    Active = d.Active,
                    IdCategory = d.IdCategory,
                    Rating = d.Rating,
                    Category = d.Category != null ? new Category
                    {
                        IdCategory = d.Category.IdCategory,
                        CategoryName = d.Category.CategoryName,
                        Description = d.Category.Description,
                        Active = d.Category.Active
                    } : null
                })
                .ToListAsync();
        }


        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.IdDish == id);
        }

        public async Task AddDishAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Entry(dish).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dish>> GetDishesByCategoryIdAsync(int categoryId)
        {
            return await _context.Dishes
                .Where(d => d.IdCategory == categoryId)
                .ToListAsync();
        }
    }
}
