using System.ComponentModel.DataAnnotations;

namespace ICEDT.API.DTO.Requst
{
    public class MediaUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        public string Folder { get; set; } = "general";
    }
} 