using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICEDT_backend_mono.Models
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeId { get; set; }
        [Required]
        public string ActivityName { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
} 