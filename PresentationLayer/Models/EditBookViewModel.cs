using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models {
    public class EditBookViewModel {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<ChapterViewModel> Chapters { get; set; } = [];

        public string CurrentImageUrl { get; set; }

        [Display(Name = "New Book Image")]
        public IFormFile? NewImage { get; set; }

        public ServerType Server {  get; set; }
    }
}