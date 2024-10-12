namespace DataAccessLayer.Entities {
    public class LibraryBook {
        public int Id { get; set; } // Primary key
        public ICollection<BookLoan> Loans { get; set; }

        public int BookId { get; set; } // Foreign key
        public Book Book { get; set; }

        public int LibraryId { get; set; } // Foreign key
        public Library Library { get; set; }
    }
}
