using BooksServiceReference;
using DataAccessLayer.Repositories;

using DbBook = DataAccessLayer.Entities.Book;
using SoapBook = BooksServiceReference.Book;

namespace BusinessLogicLayer.Services {
    public class Book : DbBook {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public List<Chapter> Chapters { get; set; } = [];
    }

    public interface IBookService {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService(IBookRepository bookRepository, BooksServiceSoapClient booksClient) : IBookService {
        public async Task<Book?> GetBookAsync(int id) {
            var dbBook = await bookRepository.GetBookAsync(id);
            if (dbBook == null) return null;

            var soapBook = (await booksClient.GetBookAsync(id)).Body.GetBookResult;

            if (soapBook == null)
                return null;

            Book book = new() {
                Id = dbBook.Id,
                Title = soapBook.Title,
                Description = soapBook.Description,
                ImageUrl = soapBook.ImageUrl,
                Chapters = soapBook.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList(),
                AuthorId = dbBook.AuthorId,
                Author = dbBook.Author,
            };

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            var dbBooks = await bookRepository.GetAllAsync();
            var soapBooks = (await booksClient.GetAllBooksAsync()).Body.GetAllBooksResult;

            List<Book> books = []; 

            foreach (var dbBook in dbBooks) {
                var soapBook = soapBooks.FirstOrDefault(b => b.Id == dbBook.Id);
                if (soapBook != null) {
                    books.Add(new() {
                        Id = dbBook.Id,
                        Title = soapBook.Title,
                        Description = soapBook.Description,
                        ImageUrl = soapBook.ImageUrl,
                        Chapters = soapBook.Chapters.Select(c => new Chapter {
                            Index = c.Index,
                            Title = c.Title,
                            Content = c.Content
                        }).ToList(),
                        AuthorId = dbBook.AuthorId,
                        Author = dbBook.Author,
                    });
                }
            }

            return books;
        }

        public async Task CreateAsync(Book book) {
            var transaction = bookRepository.BeginTransaction();

            try {
                var dbBook = await bookRepository.CreateAsync(new DbBook {
                    AuthorId = book.AuthorId,
                });

                await booksClient.CreateBookAsync(new SoapBook {
                    Id = dbBook.Id,
                    Title = book.Title,
                    Description = book.Description,
                    ImageUrl = book.ImageUrl ?? "https://birkhauser.com/product-not-found.png",
                    Chapters = book.Chapters.Select((c, index) => new Chapter {
                        Index = index,
                        Title = c.Title,
                        Content = c.Content
                    }).ToList()
                });

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
            }
        }

        public async Task UpdateAsync(Book book) {
            await booksClient.UpdateBookAsync(new SoapBook {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Chapters = book.Chapters.Select(c => new Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToList()
            });
        }

        public async Task DeleteAsync(int id) {
            var transaction = bookRepository.BeginTransaction();

            try {
                await bookRepository.DeleteAsync(id);
                await booksClient.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
            }
        }
    }
}
