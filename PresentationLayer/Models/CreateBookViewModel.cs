using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class CreateBookViewModel {

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";
        
        public List<ChapterViewModel> Chapters { get; set; } = [];

        [Display(Name = "Book Image")]
        public IFormFile? Image { get; set; }

        [Required]
        public ServerType Server { get; set; }
    }

    public class ChapterViewModel {
        public int Index { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Content { get; set; } = "";
    }
}
