using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Entities {
    public class User : IdentityUser {
        public ICollection<Book> Books { get; set; }
        public ICollection<LibraryMembership> Memberships { get; set; }
    }
}
