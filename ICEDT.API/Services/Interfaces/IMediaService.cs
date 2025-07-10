using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ICEDT.API.DTO.Requst;
using ICEDT.API.DTO.Response;

namespace ICEDT.API.Services.Interfaces
{
    public interface IMediaService
    {
        Task<MediaUploadResponseDto> UploadAsync(MediaUploadRequestDto request);
        Task DeleteAsync(string key);
        Task<MediaListResponseDto> ListAsync(string folder);
        Task<MediaUrlResponseDto> GetPresignedUrlAsync(MediaUrlRequestDto request);
    }
} 