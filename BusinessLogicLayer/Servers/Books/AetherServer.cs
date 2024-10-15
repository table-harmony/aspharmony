using ForeignBooksServiceReference;

using SoapBook = ForeignBooksServiceReference.Book;

namespace BusinessLogicLayer.Servers.Books {
    public class AetherServer(BooksServicePortTypeClient client) : IBookServer {
        public async Task<Book?> GetBookAsync(int id) {
            var response = await client.GetBookAsync(new GetBookRequest { Id = id });
            return ConvertToBook(response.GetBookResponse.book);
        }

        public async Task<List<Book>> GetAllBooksAsync() {
            var response = await client.GetAllBooksAsync(new GetAllBooksRequest());
            return response.GetAllBooksResponse1.Select(book => ConvertToBook(book)!)
                .ToList();
        }

        public async Task CreateBookAsync(Book newBook) {
            await client.CreateBookAsync(new CreateBookRequest() {
                book = ConvertToBook(newBook)
            });
        }

        public async Task UpdateBookAsync(Book updatedBook) {
            await client.UpdateBookAsync(new UpdateBookRequest() {
                book = ConvertToBook(updatedBook)
            });
        }

        public async Task DeleteBookAsync(int id) {
            await client.DeleteBookAsync(new DeleteBookRequest { Id = id });
        }

        private static Book? ConvertToBook(SoapBook? foreignBook) {
            if (foreignBook == null)
                return null;

            return new Book {
                Id = foreignBook.Id,
                Title = foreignBook.Title,
                Description = foreignBook.Description,
                ImageUrl = foreignBook.ImageUrl,
                Chapters = foreignBook.Chapters?.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList() ?? []
            };
        }

        private static SoapBook ConvertToBook(Book book) {
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