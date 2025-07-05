using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.Models
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