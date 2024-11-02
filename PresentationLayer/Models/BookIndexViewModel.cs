using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class BookIndexViewModel {
        public PaginatedList<BusinessLogicLayer.Services.Book> Books { get; set; }
        public string SearchString { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
