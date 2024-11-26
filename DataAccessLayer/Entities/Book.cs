using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities {
    public enum ServerType {
        Aether,
        Atlas,
        Dummy,
        Echo,
        Harmony,
        [Display(Name = "Nimbus 1.0")]
        Nimbus1,
        [Display(Name = "Nimbus 2.0")]
        Nimbus2,
        Orion,
        Solace,
        [Display(Name = "Stegan 1.0")]
        Stegan1,
        [Display(Name = "Stegan 2.0")]
        Stegan2,
    }

    public class Book {
        public int Id { get; set; }  // Primary key

        public ServerType Server {  get; set; }

        public string AuthorId { get; set; }  // Foreign key

        public User Author { get; set; }

        public ICollection<LibraryBook> LibraryBooks { get; set; } = [];
        public ICollection<AudioBook> AudioBooks { get; set; } = [];
    }
}
