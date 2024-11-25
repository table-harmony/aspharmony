using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Username must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }

        [Phone]
        [StringLength(20, ErrorMessage = "Phone number must be between {2} and {1} characters long.", MinimumLength = 5)]
        [RegularExpression(@"^\+?[1-9][0-9]{7,14}$", ErrorMessage = "Please enter a valid phone number")]
        public string? PhoneNumber { get; set; }
    }
}