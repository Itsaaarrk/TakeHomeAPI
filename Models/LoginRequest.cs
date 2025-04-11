using System.ComponentModel.DataAnnotations;

namespace TakeHomeAPI.Models
{
    public class LoginRequest
    {
        [Required]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters.")]
        public required string Username { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters.")]
        public required string Password { get; set; }
    }
}
