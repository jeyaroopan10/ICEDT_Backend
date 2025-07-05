using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICEDT.API.Models
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        [Required]
        public int LessonId { get; set; }
        [Required]
        public int ActivityTypeId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int SequenceOrder { get; set; }
        [Required]
        public string ContentJson { get; set; }

        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
        [ForeignKey("ActivityTypeId")]
        public ActivityType ActivityType { get; set; }
     
    }
} 