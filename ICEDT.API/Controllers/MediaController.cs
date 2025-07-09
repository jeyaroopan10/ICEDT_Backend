using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Upload( IFormFile file, [FromForm] string folder)
        {
            try
            {
                var key = await _mediaService.UploadAsync(file, folder);
                return Ok(new { key });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            try
            {
                await _mediaService.DeleteAsync(key);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] string folder)
        {
            try
            {
                var keys = await _mediaService.ListAsync(folder);
                return Ok(keys);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("url")]
        public async Task<IActionResult> GetPresignedUrl([FromQuery] string key, [FromQuery] int expiryMinutes = 60)
        {
            try
            {
                var url = await _mediaService.GetPresignedUrlAsync(key, expiryMinutes);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 