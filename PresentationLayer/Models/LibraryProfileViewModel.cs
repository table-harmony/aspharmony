using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class LibraryProfileViewModel {
        public LibraryMembership Membership { get; set; } = null!;
        public IEnumerable<BookLoan> RequestedLoans { get; set; } = [];
        public IEnumerable<BookLoan> ActiveLoans { get; set; } = [];
        public IEnumerable<BookLoan> PastLoans { get; set; } = [];
    }
}