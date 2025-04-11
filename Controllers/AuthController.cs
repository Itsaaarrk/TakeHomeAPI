using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TakeHomeAPI.Data;
using TakeHomeAPI.Models;

namespace TakeHomeAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("api/{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required.");

            var exists = await _context.Users.AnyAsync(u => u.Username == request.Username);
            if (exists)
                return BadRequest("User already exists.");

            // Compute the password hash
            var passwordHash = ComputeHash(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Username and password are required.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || user.PasswordHash != ComputeHash(request.Password))
                return Unauthorized("Invalid username or password.");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettingsSection = _configuration.GetSection("Jwt");
            var key = jwtSettingsSection.GetValue<string>("Key");
            var issuer = jwtSettingsSection.GetValue<string>("Issuer");
            var audience = jwtSettingsSection.GetValue<string>("Audience");
            var expireMinutes = jwtSettingsSection.GetValue<int>("ExpireMinutes");

            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT Key is not configured properly.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer,
                    audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(expireMinutes),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
