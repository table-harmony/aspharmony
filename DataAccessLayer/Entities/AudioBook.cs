namespace DataAccessLayer.Entities {
    public class AudioBook {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public required string AudioUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}