using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class DeleteViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}