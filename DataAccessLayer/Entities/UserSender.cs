namespace DataAccessLayer.Entities {
    public class UserSender {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public User User { get; set; }

        public int SenderId { get; set; }
        public Sender Sender { get; set; }
    }
}
