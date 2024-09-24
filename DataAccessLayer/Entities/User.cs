using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataAccessLayer.Entities {
    public class User : IdentityUser {
        public ICollection<Book> Books { get; set; }
        public ICollection<LibraryMembership> Memberships { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
