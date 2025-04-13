using System.ComponentModel.DataAnnotations;

namespace Souq.Api.Dtos.Identity
{
    public class LogInDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{6,}$",
           ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 6 characters long.")]
        public string Password { get; set; } = null!;
    }
}
