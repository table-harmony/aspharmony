using ForeignBooksServiceReference;

using SoapBook = ForeignBooksServiceReference.Book;

namespace Utils.Books {

    public class ForeignBooksService(BooksServicePortTypeClient client) : IBooksWebService {
        public async Task<Book> GetBookAsync(int id) {
            var response = await client.GetBookAsync(new GetBookRequest { Id = id });
            return ConvertToBook(response.GetBookResponse.book);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var response = await client.GetAllBooksAsync(new GetAllBooksRequest());
            return response.GetAllBooksResponse1.Select(ConvertToBook).ToList();
        }

        public async Task CreateBookAsync(Book newBook) {
            await client.CreateBookAsync(new CreateBookRequest() { 
                book = ConvertToSoapBook(newBook) 
            });
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await client.UpdateBookAsync(new UpdateBookRequest() { 
                book = ConvertToSoapBook(updatedBook) 
            });
        }

        public async Task DeleteBookAsync(int id) {
            await client.DeleteBookAsync(new DeleteBookRequest { Id = id });
        }

        private static Book ConvertToBook(SoapBook foreignBook) {
            return new Book {
                Id = foreignBook.Id,
                Title = foreignBook.Title,
                Description = foreignBook.Description,
                ImageUrl = foreignBook.ImageUrl,
                Chapters = foreignBook.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
            };
        }

        private static SoapBook ConvertToSoapBook(Book book) {
            return new SoapBook {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Chapters = book.Chapters.Select(c => new ForeignBooksServiceReference.Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToArray()
            };
        }
    }
}