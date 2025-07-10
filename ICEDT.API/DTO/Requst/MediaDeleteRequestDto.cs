using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.DTO.Requst
{
    public class MediaDeleteRequestDto
    {
        [Required]
        public string Key { get; set; }
    }
} 