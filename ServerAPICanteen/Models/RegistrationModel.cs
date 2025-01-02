using System.ComponentModel.DataAnnotations;

namespace ServerAPICanteen.Models
{
    public class RegistrationModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string? Initials { get; set; }
        public string? Role { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
