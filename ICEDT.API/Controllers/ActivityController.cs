using ICEDT.API.DTO.Request;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ICEDT.API.Controllers
{
    [ApiController]
    [Route("api/activities")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service) => _service = service;

        // Activity CRUD
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Activity ID." });
            var activity = await _service.GetActivityAsync(id);
            return Ok(activity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _service.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("by-lesson/{lessonId}")]
        public async Task<IActionResult> GetActivitiesByLessonId(int lessonId)
        {
            if (lessonId <= 0)
                return BadRequest(new { message = "Invalid Lesson ID." });
            var activities = await _service.GetActivitiesByLessonIdAsync(lessonId);
            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            var activity = await _service.AddActivityAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = activity.ActivityId }, activity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            await _service.UpdateActivityAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteActivityAsync(id);
            return NoContent();
        }

        // ActivityType CRUD
        [HttpGet("types/{id}")]
        public async Task<IActionResult> GetType(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid ActivityType ID." });
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
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            var type = await _service.AddActivityTypeAsync(dto);
            return CreatedAtAction(nameof(GetType), new { id = type.ActivityTypeId }, type);
        }

        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateType(int id, [FromBody] ActivityTypeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            await _service.UpdateActivityTypeAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            await _service.DeleteActivityTypeAsync(id);
            return NoContent();
        }
    }
}