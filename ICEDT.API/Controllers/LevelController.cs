

using ICEDT.API.DTO.Request;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT_backend_mono.Controllers
{
    [ApiController]
    [Route("api/levels")]
    public class LevelController : ControllerBase
    {
        private readonly ILevelService _service;

        public LevelController(ILevelService service) => _service = service;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var level = await _service.GetLevelAsync(id);
            return Ok(level);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var levels = await _service.GetAllLevelsAsync();
            return Ok(levels);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LevelRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            var level = await _service.AddLevelAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = level.LevelId }, level);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LevelRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            await _service.UpdateLevelAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteLevelAsync(id);
            return NoContent();
        }

        [HttpPost("{levelId}/lessons")]
        public async Task<IActionResult> AddLesson(int levelId, [FromBody] LessonRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });
            var lesson = await _service.AddLessonToLevelAsync(levelId, dto);
            return CreatedAtAction(nameof(GetLevelWithLessons), new { levelId }, lesson);
        }

        [HttpDelete("{levelId}/lessons/{lessonId}")]
        public async Task<IActionResult> RemoveLesson(int levelId, int lessonId)
        {
            await _service.RemoveLessonFromLevelAsync(levelId, lessonId);
            return NoContent();
        }

        [HttpGet("{levelId}/with-lessons")]
        public async Task<IActionResult> GetLevelWithLessons(int levelId)
        {
            if (levelId <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var result = await _service.GetLevelWithLessonsAsync(levelId);
            return Ok(result);
        }
    }
}