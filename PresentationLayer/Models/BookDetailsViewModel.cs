using DataAccessLayer.Entities;

public class BookDetailsViewModel {
    public LibraryBook LibraryBook { get; set; }
    public IEnumerable<BookLoan> PastLoans { get; set; }
    public BookLoan CurrentLoan { get; set; }
    public bool CanBeBorrowed { get; set; }
}