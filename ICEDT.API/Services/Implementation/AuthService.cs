using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ICEDT.API.DTO.Request;
using ICEDT.API.Models;
using ICEDT.API.Services.Interfaces;

namespace ICEDT.API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(bool Success, string Token, string ErrorMessage)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: User with email {Email} not found.", loginDto.Email);
                    return (false, null, "Invalid email or password.");
                }

                // Verify password
                if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    _logger.LogWarning("Login failed: Invalid password for {Email}.", loginDto.Email);
                    return (false, null, "Invalid email or password.");
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                // Add roles to claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.UtcNow.AddDays(1); // Token valid for 1 day

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: expiry,
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
                return (true, tokenString, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Email}.", loginDto.Email);
                return (false, null, "An unexpected error occurred.");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: User with email {Email} already exists.", registerDto.Email);
                    return (false, "User with this email already exists.");
                }

                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    ProfileImage = registerDto.ProfileImage
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Registration failed for {Email}: {Errors}", registerDto.Email, errors);
                    return (false, errors);
                }

                _logger.LogInformation("User {Email} registered successfully.", registerDto.Email);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration for {Email}.", registerDto.Email);
                return (false, "An unexpected error occurred.");
            }
        }
    }
}