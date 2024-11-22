using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class CreateBookViewModel {

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Cover Image")]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB max
        public IFormFile? Image { get; set; }

        public ServerType Server { get; set; }

        [Display(Name = "Chapters")]
        public List<ChapterViewModel> Chapters { get; set; } = new();

        [Display(Name = "Generate AI Cover Image")]
        public bool GenerateImage { get; set; }
    }

    public class ChapterViewModel {
        public int Index { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        [Required]
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
