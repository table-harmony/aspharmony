using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public class Book {
        public int Id { get; set; }  // Primary key
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public User Author { get; set; }

        public ICollection<LibraryBook> LibraryBooks { get; set; }
    }
}
