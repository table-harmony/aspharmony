using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities {
    public class LibraryBook {
        public int Id { get; set; }

        public Book Book { get; set; }
        public int BookId { get; set; }

        public Library Library { get; set; }
        public int LibraryId { get; set; }

        public ICollection<BookLoan> Loans { get; set; }
    }
}
