

namespace ICEDT.API.DTO.Response
{
    public class LevelWithLessonsResponseDto
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int SequenceOrder { get; set; }
        public List<LessonResponseDto> Lessons { get; set; }
    }
} 