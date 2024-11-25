using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models {
    public class BookDetailsViewModel {
        [Required(ErrorMessage = "Library book information is required")]
        public LibraryBook LibraryBook { get; set; } = null!;

        [Required(ErrorMessage = "Book information is required")]
        public Book Book { get; set; } = null!;

        public BookLoan? CurrentLoan { get; set; }

        [Display(Name = "Loan History")]
        public IEnumerable<BookLoan> PastLoans { get; set; } = [];

        [Display(Name = "Other Available Copies")]
        public IEnumerable<LibraryBook> OtherCopies { get; set; } = [];

        public BookLoan? ActiveLoan { get; set; }

        [Display(Name = "Queue Position")]
        [Range(0, int.MaxValue, ErrorMessage = "Queue position must be a non-negative number")]
        public int QueuePosition { get; set; }

        [Display(Name = "Current Queue")]
        public IEnumerable<BookLoan> Queue { get; set; } = [];

        [Display(Name = "Notes")]
        [StringLength(1000, ErrorMessage = "Notes cannot exceed {1} characters")]
        public string? Notes { get; set; }

        [Display(Name = "Condition")]
        [Range(1, 5, ErrorMessage = "Condition rating must be between {1} and {2}")]
        public int? Condition { get; set; }
    }
}