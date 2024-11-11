using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bahrin_Harbour.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        [HttpGet("getimage")]
        public IActionResult GetImage(string folderName, string fileName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", folderName);
            string filePath = Path.Combine(folderPath, fileName);

            if(!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }
    }
}
