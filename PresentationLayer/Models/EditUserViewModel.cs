using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string? PhoneNumber {  get; set; }
    }
}