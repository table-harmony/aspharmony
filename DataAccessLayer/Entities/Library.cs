using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public class Library {
        public int Id { get; set; }  // Primary key
        public string Name { get; set; }

        public ICollection<LibraryBook> Books { get; set; }
        public ICollection<LibraryMembership> Memberships { get; set; }
    }
}
