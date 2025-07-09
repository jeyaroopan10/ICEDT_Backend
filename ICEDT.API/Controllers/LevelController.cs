

using ICEDT.API.DTO.Request;
using ICEDT.API.Models;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _service;

        public LevelsController(ILevelService service) => _service = service;

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var level = await _service.GetLevelAsync(id);
            return Ok(level);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllLevelsAsync());


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LevelRequestDto dto)
        {
            var level = await _service.AddLevelAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = level.LevelId }, level);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LevelRequestDto dto)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            await _service.UpdateLevelAsync(id, dto);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            await _service.DeleteLevelAsync(id);
            return NoContent();
        }


        [HttpPost("{levelId:int}/lessons")]
        public async Task<IActionResult> AddLesson(int levelId, [FromBody] LessonRequestDto dto)
        {
            if (levelId <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var lesson = await _service.AddLessonToLevelAsync(levelId, dto);
            return CreatedAtAction(nameof(GetLevelWithLessons), new { levelId }, lesson);
        }

        [HttpDelete("{levelId:int}/lessons/{lessonId:int}")]
        public async Task<IActionResult> RemoveLesson(int levelId, int lessonId)
        {
            if (levelId <= 0 || lessonId <= 0)
                    return BadRequest(new { message = "Invalid  ID." });
            await _service.RemoveLessonFromLevelAsync(levelId, lessonId);
            return NoContent();
        }

        [HttpGet("{levelId:int}/with-lessons")]
        public async Task<IActionResult> GetLevelWithLessons(int levelId)
        {
            if (levelId <= 0)
                return BadRequest(new { message = "Invalid Level ID." });
            var result = await _service.GetLevelWithLessonsAsync(levelId);
            return Ok(result);
        }
    }
}