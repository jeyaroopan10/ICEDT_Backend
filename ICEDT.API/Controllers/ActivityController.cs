using ICEDT.API.DTO.Request;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ICEDT.API.Middleware;


namespace ICEDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivitiesController(IActivityService service) => _service = service;

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            var activity = await _service.GetActivityAsync(id);
            return Ok(activity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllActivitiesAsync());

        [HttpGet("by-lesson")]
        public async Task<IActionResult> GetActivitiesByLessonId(
            [FromQuery] int lessonId,
            [FromQuery] int? activityTypeId,
            [FromQuery] int? mainActivityTypeId)
        {
            if (lessonId <= 0)
                throw new BadRequestException("Invalid Lesson ID.");
            var activities = await _service.GetActivitiesByLessonIdAsync(lessonId, activityTypeId, mainActivityTypeId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityRequestDto dto)
        {
            var activity = await _service.AddActivityAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.UpdateActivityAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid Activity ID.");
            await _service.DeleteActivityAsync(id);
            return NoContent();
        }

        // ActivityType CRUD

        [HttpGet("types/{id:int}")]
        public async Task<IActionResult> GetType(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            var type = await _service.GetActivityTypeAsync(id);
            return Ok(type);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _service.GetAllActivityTypesAsync();
            return Ok(types);
        }

        [HttpPost("types")]
        public async Task<IActionResult> CreateType([FromBody] ActivityTypeRequestDto dto)
        {
            var type = await _service.AddActivityTypeAsync(dto);
            return CreatedAtAction(nameof(GetType), new { id = type.ActivityTypeId }, type);
        }

        [HttpPut("types/{id:int}")]
        public async Task<IActionResult> UpdateType(int id, [FromBody] ActivityTypeRequestDto dto)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.UpdateActivityTypeAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("types/{id:int}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            if (id <= 0)
                throw new BadRequestException("Invalid ActivityType ID.");
            await _service.DeleteActivityTypeAsync(id);
            return NoContent();
        }
    }
}