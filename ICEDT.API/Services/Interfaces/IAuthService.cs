using ICEDT.API.DTO.Request;

namespace ICEDT.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterDto registerDto);
    }
}