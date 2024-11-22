namespace DataAccessLayer.Entities {
    public class BookLoan {
        public int Id { get; set; }

        public int LibraryBookId { get; set; }
        public LibraryBook LibraryBook { get; set; }

        public int LibraryMembershipId { get; set; }
        public LibraryMembership LibraryMembership { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public LoanStatus Status { get; set; }

        public DateTime? ExpectedReturnDate => LoanDate?.AddDays(14);
    }

    public enum LoanStatus {
        Requested,
        Active,
        Completed
    }
}