using DataAccessLayer.Entities;
using BusinessLogicLayer.Services;
using System.ComponentModel.DataAnnotations;

using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Models
{
    public class AddBookViewModel {
        [Required(ErrorMessage = "Library is required")]
        public Library Library { get; set; }

        [Required(ErrorMessage = "At least one book must be available")]
        [Display(Name = "Available Books")]
        public List<Book> AvailableBooks { get; set; } = [];
        
        [Required(ErrorMessage = "Please select a book")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Number of copies is required")]
        [Range(1, 100, ErrorMessage = "Number of copies must be between {1} and {2}")]
        [Display(Name = "Number of Copies")]
        public int Copies { get; set; } = 1;
    }
} 