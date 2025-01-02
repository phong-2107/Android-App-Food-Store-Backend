using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPICanteen.Models;
using ServerAPICanteen.Repositories;

namespace ServerAPICanteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanteenApiController : ControllerBase
    {
        private readonly ICanteenRepository _canteenRepository;

        public CanteenApiController(ICanteenRepository canteenRepository)
        {
            _canteenRepository = canteenRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCanteens()
        {
            try
            {
                var canteens = await _canteenRepository.GetCanteensAsync();
                return Ok(canteens);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCanteenById(int id)
        {
            try
            {
                var canteen = await _canteenRepository.GetCanteenByIdAsync(id);
                if (canteen == null)
                    return NotFound();
                return Ok(canteen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCanteen([FromBody] Canteen canteen)
        {
            try
            {
                await _canteenRepository.AddCanteenAsync(canteen);
                return CreatedAtAction(nameof(GetCanteenById), new { id = canteen.IdCanteen }, canteen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCanteen(int id, [FromBody] Canteen canteen)
        {
            try
            {
                if (id != canteen.IdCanteen)
                    return BadRequest();
                await _canteenRepository.UpdateCanteenAsync(canteen);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCanteen(int id)
        {
            try
            {
                await _canteenRepository.DeleteCanteenAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
