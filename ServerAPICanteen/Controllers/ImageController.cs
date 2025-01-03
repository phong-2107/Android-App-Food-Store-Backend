using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using ServerAPICanteen.Models;

namespace ServerAPICanteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public ImageController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid image file.");

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Transformation = new Transformation().Crop("fill").Gravity("face").Width(500).Height(500)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                return StatusCode(500, "Error uploading image to Cloudinary.");

            return Ok(new { url = uploadResult.Url.ToString() });
        }
    }
}
