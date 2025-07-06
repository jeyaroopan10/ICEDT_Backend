using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.DTO.Request
{
    public class ActivityRequestDto
    {
        [Required(ErrorMessage = "Lesson ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Lesson ID must be a positive number.")]
        public int LessonId { get; set; }

        [Required(ErrorMessage = "Activity Type ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Activity Type ID must be a positive number.")]
        public int ActivityTypeId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Sequence order is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sequence order must be a positive number.")]
        public int SequenceOrder { get; set; }

        [Required(ErrorMessage = "Content JSON is required.")]
        public string ContentJson { get; set; }
    }
} 