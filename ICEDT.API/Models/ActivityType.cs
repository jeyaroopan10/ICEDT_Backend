using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.Models
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MainActivityTypeId { get; set; } // Foreign key
        public MainActivityType? MainActivityType { get; set; } // Navigational property
        public ICollection<Activity>? Activities { get; set; }
    }
}