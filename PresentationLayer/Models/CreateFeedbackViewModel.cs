using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class CreateFeedbackViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public Label Label { get; set; }
    }
}