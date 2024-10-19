namespace DataAccessLayer.Entities {

    public enum ServerType {
        Aether,
        Atlas,
        Dummy,
        Echo,
        Harmony,
        Nimbus,
        Orion,
        Solace,
        Stegan,
    }

    public class Book {
        public int Id { get; set; }  // Primary key

        public ServerType Server {  get; set; }

        public string AuthorId { get; set; }  // Foreign key
        public User Author { get; set; }

        public ICollection<LibraryBook> LibraryBooks { get; set; }
    }
}
