using ICEDT.API.Services.Interfaces;
using ICEDT.API.DTO.Requst;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(MediaUploadRequestDto request)
        {
            var result = await _mediaService.UploadAsync(request);
            return Ok(result);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest(new { message = "Key is required." });

            await _mediaService.DeleteAsync(key);
            return NoContent();
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] string folder = "")
        {
            var result = await _mediaService.ListAsync(folder);
            return Ok(result);
        }

        [HttpGet("url")]
        public async Task<IActionResult> GetPresignedUrl([FromQuery] string key, [FromQuery] int expiryMinutes = 60)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest(new { message = "Key is required." });

            var request = new MediaUrlRequestDto
            {
                Key = key,
                ExpiryMinutes = expiryMinutes
            };

            var result = await _mediaService.GetPresignedUrlAsync(request);
            return Ok(result);
        }
    }
}
