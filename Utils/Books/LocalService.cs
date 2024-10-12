using LocalBooksServiceReference;

using SoapBook = LocalBooksServiceReference.Book;

namespace Utils.Books {

    public class LocalBooksService(BooksServiceSoapClient client) : IBooksWebService {
        public async Task<Book> GetBookAsync(int id) {
            var response = await client.GetBookAsync(id);
            return ConvertToBook(response.Body.GetBookResult);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var response = await client.GetAllBooksAsync();
            return response.Body.GetAllBooksResult.Select(ConvertToBook).ToList();
        }

        public async Task CreateBookAsync(Book newBook) {
            await client.CreateBookAsync(ConvertToSoapBook(newBook));
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await client.UpdateBookAsync(ConvertToSoapBook(updatedBook));
        }

        public async Task DeleteBookAsync(int id) {
            await client.DeleteBookAsync(id);
        }

        private static Book ConvertToBook(SoapBook soapBook) {
            return new Book {
                Id = soapBook.Id,
                Title = soapBook.Title,
                Description = soapBook.Description,
                ImageUrl = soapBook.ImageUrl,
                Chapters = soapBook.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
            };
        }

        private static SoapBook ConvertToSoapBook(Book book) {
            return new SoapBook {
                Id = book.Id,
                Title =  book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Chapters = book.Chapters.Select(c => new LocalBooksServiceReference.Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
            };
        }
    }
}