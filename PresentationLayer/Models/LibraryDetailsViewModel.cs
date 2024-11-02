using DataAccessLayer.Entities;
using Book = BusinessLogicLayer.Services.Book;

namespace PresentationLayer.Models {
    public class LibraryDetailsViewModel {
        public Library Library { get; set; }
        public PaginatedList<LibraryBook> Books { get; set; }
    }

    public class ManageBooksViewModel {
        public Library Library { get; set; }
        public List<Book> AvailableBooks { get; set; }
        public PaginatedList<LibraryBook> Books { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; } = 10;
    }

    public class ManageMembersViewModel {
        public Library Library { get; set; }
        public PaginatedList<LibraryMembership> Members { get; set; }
        public string SearchString { get; set; }
    }
}