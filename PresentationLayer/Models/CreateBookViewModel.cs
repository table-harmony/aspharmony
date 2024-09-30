using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PresentationLayer.Models
{
    public class CreateBookViewModel {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<ChapterViewModel> Chapters { get; set; } = new List<ChapterViewModel>();

        [Display(Name = "Book Image")]
        public IFormFile? Image { get; set; }

    }

    public class ChapterViewModel {
        public int Index { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
    }
}