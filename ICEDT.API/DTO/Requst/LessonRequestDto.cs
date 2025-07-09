using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.DTO.Request
{
    public class LessonRequestDto
    {
        [Required(ErrorMessage = "Lesson name is required.")]
        [StringLength(100, ErrorMessage = "Lesson name cannot exceed 100 characters.")]
        public string LessonName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Sequence order is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence order must be a positive number.")]
        public int SequenceOrder { get; set; }
    }
} 