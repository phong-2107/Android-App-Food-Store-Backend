﻿using Microsoft.EntityFrameworkCore;
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
            return await _context.Dishes.Include(d => d.Category).ToListAsync();
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

        public async Task UpdateDishPictureAsync(int id, string pictureUrl)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                dish.PictureUrlArray = pictureUrl;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<object>> GetDishesWithCategoryAsync()
        {
            return await _context.Dishes
                .Select(dish => new
                {
                    dish.IdDish,
                    dish.DishName,
                    dish.Description,
                    dish.Amount,
                    dish.PictureUrlArray,
                    dish.Active,
                    CategoryName = dish.Category != null ? dish.Category.CategoryName : "Unknown Category"
                })
                .ToListAsync();
        }

    }
}
