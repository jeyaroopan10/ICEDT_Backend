namespace ICEDT.API.DTO.Response
{
    public class MediaUploadResponseDto
    {
        public string Key { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
        public string Message { get; set; } = "File uploaded successfully";
    }
} 