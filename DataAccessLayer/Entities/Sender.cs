namespace DataAccessLayer.Entities {
    public class Sender {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<UserSender> Users { get; set; } = [];
    }
}
