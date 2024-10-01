namespace DataAccessLayer.Entities {
    public class Feedback {
        public int Id { get; set; }  // Primary key

        public string UserId { get; set; }  // Foreign key
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}