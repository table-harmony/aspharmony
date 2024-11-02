using DataAccessLayer.Entities;
using BusinessLogicLayer.Services;
using System.ComponentModel.DataAnnotations;

using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Models
{
    public class AddBookViewModel
    {
        public Library Library { get; set; }
        public List<Book> AvailableBooks { get; set; } = [];
        
        [Required]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Range(1, 100)]
        [Display(Name = "Number of Copies")]
        public int Copies { get; set; } = 1;
    }
} 