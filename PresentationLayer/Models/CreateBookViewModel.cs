using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class CreateBookViewModel {

        [Required]
        [StringLength(100, ErrorMessage = "The title must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, ErrorMessage = "The description must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Display(Name = "Cover Image")]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB max
        public IFormFile? Image { get; set; }

        public ServerType Server { get; set; }

        [Display(Name = "Chapters")]
        public List<ChapterViewModel> Chapters { get; set; } = [];

        [Display(Name = "Generate AI Cover Image")]
        public bool GenerateImage { get; set; }
    }

    public class ChapterViewModel {
        public int Index { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The chapter title must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(50000, ErrorMessage = "The chapter content must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; } = "";
    }

    public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (value is IFormFile file) {
                if (file.Length > maxFileSize) {
                    return new ValidationResult($"File size cannot exceed {maxFileSize / 1024 / 1024}MB");
                }
            }
            return ValidationResult.Success;
        }
    }
}
