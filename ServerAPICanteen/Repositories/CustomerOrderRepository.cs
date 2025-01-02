using Microsoft.EntityFrameworkCore;
using ServerAPICanteen.Models;

namespace ServerAPICanteen.Repositories
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerOrder> GetCustomerOrderByIdAsync(int id)
        {
            return await _context.CustomerOrders
                                 .Include(co => co.User)
                                 .Include(co => co.Canteen)
                                 .Include(co => co.OrderDetails)
                                 .FirstOrDefaultAsync(co => co.IdOrder == id);
        }

        public async Task CreateCustomerOrderAsync(CustomerOrder customerOrder)
        {
            _context.CustomerOrders.Add(customerOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerOrderAsync(CustomerOrder customerOrder)
        {
            _context.Entry(customerOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerOrderAsync(int id)
        {
            var customerOrder = await _context.CustomerOrders.FindAsync(id);
            if (customerOrder != null)
            {
                _context.CustomerOrders.Remove(customerOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
