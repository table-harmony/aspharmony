using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class LibraryViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "Library name is required")]
        [StringLength(100, ErrorMessage = "The library name must be between {2} and {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Library Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_\.]+$", ErrorMessage = "Library name can only contain letters, numbers, spaces, and basic punctuation")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please specify if multiple copies are allowed")]
        [Display(Name = "Allow Multiple Copies")]
        public bool AllowCopies { get; set; }
    }
}