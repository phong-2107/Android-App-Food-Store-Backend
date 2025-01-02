using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerAPICanteen.Models;
using ServerAPICanteen.Repositories;

namespace ServerAPICanteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerOrderApiController : ControllerBase
    {
        private readonly ICustomerOrderRepository _customerOrderRepository;
        private readonly ICanteenRepository _canteenRepository;
        private readonly IUserRepository _userRepository;

        public CustomerOrderApiController(ICustomerOrderRepository customerOrderRepository, IUserRepository userRepository, ICanteenRepository canteenRepository)
        {
            _customerOrderRepository = customerOrderRepository;
            _userRepository = userRepository;
            _canteenRepository = canteenRepository;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateCustomerOrder([FromBody] CustomerOrder customerOrder)
        {
            try
            {
                if (customerOrder == null)
                    return BadRequest("Invalid data.");

                // Kiểm tra xem Id (User ID) và IdCanteen có hợp lệ không
                if (string.IsNullOrEmpty(customerOrder.Id))
                    return BadRequest("User ID is required.");

                if (!customerOrder.IdCanteen.HasValue)
                    return BadRequest("Canteen ID is required.");

                // Kiểm tra User tồn tại
                var user = await _userRepository.GetUserByIdAsync(customerOrder.Id);
                if (user == null)
                    return NotFound("User not found.");

                // Kiểm tra Canteen tồn tại
                var canteen = await _canteenRepository.GetCanteenByIdAsync(customerOrder.IdCanteen.Value);
                if (canteen == null)
                    return NotFound("Canteen not found.");

                // Thiết lập các liên kết navigation properties
                customerOrder.User = user;
                customerOrder.Canteen = canteen;

                // Tạo CustomerOrder
                await _customerOrderRepository.CreateCustomerOrderAsync(customerOrder);

                return CreatedAtAction(nameof(GetCustomerOrderById), new { id = customerOrder.IdOrder }, customerOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCustomerOrder(int id, [FromBody] CustomerOrder customerOrder)
        {
            try
            {
                if (id != customerOrder.IdOrder)
                    return BadRequest("ID mismatch.");

                await _customerOrderRepository.UpdateCustomerOrderAsync(customerOrder);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCustomerOrder(int id)
        {
            try
            {
                await _customerOrderRepository.DeleteCustomerOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerOrderById(int id)
        {
            try
            {
                var order = await _customerOrderRepository.GetCustomerOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
