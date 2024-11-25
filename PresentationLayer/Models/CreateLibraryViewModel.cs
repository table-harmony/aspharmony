using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class CreateLibraryViewModel {
        [Required]
        [StringLength(100, ErrorMessage = "The library name must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }
    }
}