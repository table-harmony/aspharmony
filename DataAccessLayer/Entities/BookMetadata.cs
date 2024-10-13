namespace DataAccessLayer.Entities {
    public class BookMetadata {
        
        public int Id { get; set; }  // Primary key

        public int BookId { get; set; }  // Foreign key
        public Book Book { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
