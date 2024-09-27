namespace DataAccessLayer.Entities {
    public class Notification {
        public int Id { get; set; }  // Primary key
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }  // Foreign key
        public User User { get; set; }
    }
}