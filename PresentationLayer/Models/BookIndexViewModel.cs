using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models {
    public class BookIndexViewModel {
        [Required(ErrorMessage = "Books collection is required")]
        public PaginatedList<BusinessLogicLayer.Services.Book> Books { get; set; }

        [Display(Name = "Search")]
        [StringLength(100, ErrorMessage = "Search string cannot exceed {1} characters")]
        public string SearchString { get; set; }

        [Required(ErrorMessage = "Page size is required")]
        [Range(1, 100, ErrorMessage = "Page size must be between {1} and {2}")]
        [Display(Name = "Items per page")]
        public int PageSize { get; set; } = 10;

        [Display(Name = "Sort By")]
        public string? SortBy { get; set; }

        [Display(Name = "Filter By")]
        public string? FilterBy { get; set; }
    }
}
