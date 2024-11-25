using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class DeleteViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}