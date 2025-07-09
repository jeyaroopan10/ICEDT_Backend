using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ICEDT.API.Services.Interfaces
{
    public interface IMediaService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        Task DeleteAsync(string key);
        Task<IList<string>> ListAsync(string folder);
        Task<string> GetPresignedUrlAsync(string key, int expiryMinutes = 60);
    }
} 