using System.ServiceModel;

namespace WebServices.net8.Services.Books;

[ServiceContract]
public interface IBooksService {
    [OperationContract]
    Book? GetBook(int id);

    [OperationContract]
    IEnumerable<Book> GetAllBooks();

    [OperationContract]
    void CreateBook(Book newBook);

    [OperationContract]
    void UpdateBook(Book updatedBook);

    [OperationContract]
    void DeleteBook(int id);
}


public class Book {
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public List<Chapter> Chapters { get; set; } = [];
}

public class Chapter {
    public int Index { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}

