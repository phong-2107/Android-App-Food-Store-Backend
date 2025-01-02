using Microsoft.EntityFrameworkCore;
using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public class CanteenRepository : ICanteenRepository
    {
        private readonly ApplicationDbContext _context;

        public CanteenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Canteen>> GetCanteensAsync()
        {
            return await _context.Canteens.Include(c => c.CustomerOrders).ToListAsync();
        }

        public async Task<Canteen> GetCanteenByIdAsync(int id)
        {
            return await _context.Canteens.Include(c => c.CustomerOrders)
                                          .FirstOrDefaultAsync(c => c.IdCanteen == id);
        }

        public async Task AddCanteenAsync(Canteen canteen)
        {
            _context.Canteens.Add(canteen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCanteenAsync(Canteen canteen)
        {
            _context.Entry(canteen).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCanteenAsync(int id)
        {
            var canteen = await _context.Canteens.FindAsync(id);
            if (canteen != null)
            {
                _context.Canteens.Remove(canteen);
                await _context.SaveChangesAsync();
            }
        }
    }
}
