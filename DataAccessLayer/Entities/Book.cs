namespace DataAccessLayer.Entities {
    public class Book {
        public int Id { get; set; }  // Primary key

        public int ServerId { get; set; } // Foreign key
        public Server Server { get; set; }

        public string AuthorId { get; set; }  // Foreign key
        public User Author { get; set; }

        public ICollection<LibraryBook> LibraryBooks { get; set; }
    }
}
