using Microsoft.EntityFrameworkCore;
using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public class OrderMRepository : IOrderMRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderMRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerOrder>> GetAllAsync()
        {
            return await _context.CustomerOrders
                .Include(o => o.Canteen)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<CustomerOrder?> GetByIdAsync(int id)
        {
            return await _context.CustomerOrders
                .Include(o => o.Canteen)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.IdOrder == id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(od => od.IdOrder == orderId)
                .Include(od => od.Dish) // Include related Dish
                .ToListAsync();
        }
    }
}
