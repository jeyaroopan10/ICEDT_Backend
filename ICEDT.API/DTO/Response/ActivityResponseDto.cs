namespace ICEDT.API.DTO.Response
{
    public class ActivityResponseDto
    {
        public int ActivityId { get; set; }
        public int LessonId { get; set; }
        public int ActivityTypeId { get; set; }
        public int MainActivityTypeId { get; set; }
        public string Title { get; set; }
        public int SequenceOrder { get; set; }
        public string ContentJson { get; set; }
    }
} 