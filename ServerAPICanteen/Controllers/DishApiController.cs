using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPICanteen.Models;
using ServerAPICanteen.Repositories;

namespace ServerAPICanteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishApiController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;

        public DishApiController(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDishes()
        {
            try
            {
                var dishes = await _dishRepository.GetDishesAsync();
                var simplifiedDishes = dishes.Select(dish => new
                {
                    dish.IdDish,
                    dish.DishName,
                    dish.PictureUrlArray,
                    dish.Amount,
                    dish.DishStats,
                    dish.Description,
                    dish.Active,
                    dish.IdCategory,
                    dish.Rating,
                    category = dish.Category == null ? null : new
                    {
                        dish.Category.IdCategory,
                        dish.Category.CategoryName,
                        dish.Category.Description,
                        dish.Category.Active
                    }
                });

                return Ok(simplifiedDishes);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("ordered-by-dish-rating")]
        public async Task<IActionResult> GetDishesOrderedByDishRating()
        {
            try
            {
                var dishes = await _dishRepository.GetDishesOrderedByDishRatingAsync();
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishById(int id)
        {
            try
            {
                var dish = await _dishRepository.GetDishByIdAsync(id);
                if (dish == null)
                    return NotFound();
                return Ok(dish);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("category/{id}")]
        public async Task<IActionResult> GetDishesByCategoryId(int id)
        {
            try
            {
                var dishes = await _dishRepository.GetDishesByCategoryIdAsync(id);
                if (dishes == null || !dishes.Any())
                    return NotFound("No dishes found for the given category ID.");
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDish([FromBody] Dish dish)
        {
            try
            {
                await _dishRepository.AddDishAsync(dish);
                return CreatedAtAction(nameof(GetDishById), new { id = dish.IdDish }, dish);
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] Dish dish)
        {
            try
            {
                if (id != dish.IdDish)
                    return BadRequest();
                await _dishRepository.UpdateDishAsync(dish);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            try
            {
                await _dishRepository.DeleteDishAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Handle exception
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
