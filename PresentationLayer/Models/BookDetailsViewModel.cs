using DataAccessLayer.Entities;

namespace PresentationLayer.Models
{
    public class BookDetailsViewModel {
        public LibraryBook LibraryBook { get; set; }
        public Book Book { get; set; }
        public IEnumerable<BookLoan> PastLoans { get; set; }
        public BookLoan? CurrentLoan { get; set; }
    }
}