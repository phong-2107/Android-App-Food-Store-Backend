using Microsoft.AspNetCore.Mvc;
using ServerAPICanteen.Models;
using ServerAPICanteen.Repositories;

namespace ServerAPICanteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderMController : ControllerBase
    {
        private readonly IOrderMRepository _orderRepository;

        public OrderMController(IOrderMRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetOrderDetailsByOrderId(int id)
        {
            var orderDetails = await _orderRepository.GetOrderDetailsByOrderIdAsync(id);
            if (orderDetails == null || !orderDetails.Any())
                return NotFound(new { message = "No order details found for the given order ID." });

            return Ok(orderDetails);
        }
    }
}
