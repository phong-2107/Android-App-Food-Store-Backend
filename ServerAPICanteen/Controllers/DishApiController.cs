using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
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
                return Ok(dishes);
            }
            catch (Exception ex)
            {
                // Handle exception
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

        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadDishImage(int id, IFormFile file, [FromServices] Cloudinary cloudinary)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid image file.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation().Crop("fill").Gravity("face").Width(500).Height(500)
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return StatusCode(500, "Error uploading image to Cloudinary.");

            await _dishRepository.UpdateDishPictureAsync(id, uploadResult.Url.ToString());
            return Ok(new { url = uploadResult.Url.ToString() });
        }

    }
}
