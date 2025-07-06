using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.DTO.Request
{
    public class ActivityTypeRequestDto
    {
        [Required(ErrorMessage = "Activity type name is required.")]
        [StringLength(50, ErrorMessage = "Activity type name cannot exceed 50 characters.")]
        public string ActivityName { get; set; }
    }
} 