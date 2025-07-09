using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.Models
{
    public class MainActivityType
    {
        [Key]
        public int MainActivityTypeId { get; set; }
        [Required]
        public string Name { get; set; }

        public List<ActivityType> ?ActivityTypes { get; set; } 
        public ICollection<Activity> ?Activities { get; set; }
    }
}
