using DataAccessLayer.Entities;

public class BookLoan {
    public int Id { get; set; }
    public int LibraryBookId { get; set; }
    public LibraryBook LibraryBook { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int LibraryId { get; set; }
    public Library Library { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}