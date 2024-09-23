using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}