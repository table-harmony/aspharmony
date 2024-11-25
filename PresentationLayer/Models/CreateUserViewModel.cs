using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class CreateUserViewModel {

        [Required]
        [StringLength(50, ErrorMessage = "Username must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The password must be between {2} and {1} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Phone]
        [StringLength(20, ErrorMessage = "Phone number must be between {2} and {1} characters long.", MinimumLength = 5)]
        [RegularExpression(@"^\+?[1-9][0-9]{7,14}$", ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

    }
}
