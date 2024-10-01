using DataAccessLayer.Entities;

public class BookLoan {
    public int Id { get; set; } // Primary key

    public int LibraryBookId { get; set; }  // Foreign key
    public LibraryBook LibraryBook { get; set; }

    public int LibraryMembershipId { get; set; }  // Foreign key
    public LibraryMembership LibraryMembership { get; set; }

    
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}