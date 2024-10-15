using DataAccessLayer.Entities;

namespace PresentationLayer.Models {
    public class BookIndexViewModel {
        public IEnumerable<BusinessLogicLayer.Services.Book> Books { get; set; }
        public IEnumerable<Server> Servers { get; set; }
        public int? SelectedServerId { get; set; }
    }
}
