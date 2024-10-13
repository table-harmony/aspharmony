namespace DataAccessLayer.Entities {
    public class BookChapter {

        public int Id { get; set; }  // Primary key

        public int BookId { get; set; }  // Foreign key
        public Book Book { get; set; }

        public int Index { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
