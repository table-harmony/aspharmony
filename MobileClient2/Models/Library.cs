
namespace MobileClient2.Models; 

public class Library {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllowCopies { get; set; }
    public List<LibraryBook> Books { get; set; } = [];
}

public class LibraryBook {
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
}