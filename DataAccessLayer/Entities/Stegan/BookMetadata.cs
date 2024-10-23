namespace DataAccessLayer.Entities.Stegan {
    public class BookMetadata {
        public int BookId { get; set; }  // Primary key

        public required string Url { get; set; }
    }
}
