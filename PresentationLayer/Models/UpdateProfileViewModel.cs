using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class UpdateProfileViewModel {
        [Required]
        [StringLength(50, ErrorMessage = "Username must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [StringLength(20, ErrorMessage = "Phone number must be between {2} and {1} characters long.", MinimumLength = 5)]
        [RegularExpression(@"^\+?[1-9][0-9]{7,14}$", ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
} 