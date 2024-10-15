using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class BookIndexViewModel {
        public IEnumerable<BusinessLogicLayer.Services.Book> Books { get; set; }
        public string SearchString { get; set; }
    }
}
