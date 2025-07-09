using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICEDT.API.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        [Required]
        public int LevelId { get; set; }
        [Required]
        public string LessonName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int SequenceOrder { get; set; }

        [ForeignKey("LevelId")]
        public Level Level { get; set; }
        public ICollection<Activity> ?Activities { get; set; }
     
    }
} 