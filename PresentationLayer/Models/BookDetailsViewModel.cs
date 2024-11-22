using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class BookDetailsViewModel {
        public LibraryBook LibraryBook { get; set; } = null!;
        public Book Book { get; set; } = null!;
        public BookLoan? CurrentLoan { get; set; }
        public IEnumerable<BookLoan> PastLoans { get; set; } = [];
        public IEnumerable<LibraryBook> OtherCopies { get; set; } = [];
        public BookLoan? ActiveLoan { get; set; }
        public int QueuePosition { get; set; }
        public IEnumerable<BookLoan> Queue { get; set; } = [];
    }
}