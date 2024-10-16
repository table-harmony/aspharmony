﻿namespace DataAccessLayer.Entities {

    public enum ServerType {
        Aether,
        Atlas,
        Echo,
        Nimbus,
        Orion,
        Solace,
    }

    public class Book {
        public int Id { get; set; }  // Primary key

        public ServerType Server {  get; set; }

        public string AuthorId { get; set; }  // Foreign key
        public User Author { get; set; }

        public ICollection<LibraryBook> LibraryBooks { get; set; }
    }
}
