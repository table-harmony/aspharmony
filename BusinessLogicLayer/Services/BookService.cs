using Utils.Books;
using DataAccessLayer.Repositories;

using DbBook = DataAccessLayer.Entities.Book;
using WebServiceBook = Utils.Books.Book;

namespace BusinessLogicLayer.Services {
    public class Book : DbBook {
        public WebServiceBook? Metadata { get; set; }
    }

    public interface IBookService {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService(IBookRepository repository, IBooksWebService webService) : IBookService {
        public async Task<Book?> GetBookAsync(int id) {
            DbBook? dbBook = await repository.GetBookAsync(id);
            if (dbBook == null) return null;

            WebServiceBook? webBook = await webService.GetBookAsync(id);

            if (webBook == null)
                return null;

            return new Book {
                Id = dbBook.Id,
                Author = dbBook.Author,
                AuthorId = dbBook.AuthorId,
                Metadata = webBook
            };
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            IEnumerable<DbBook> dbBooks = await repository.GetAllAsync();
            List<WebServiceBook> webBooks = await webService.GetAllBooksAsync();

            var books = dbBooks.GroupJoin(webBooks,
                db => db.Id,
                web => web.Id,
                (db, webMatches) => new Book {
                    Id = db.Id,
                    Author = db.Author,
                    AuthorId = db.AuthorId,
                    Metadata = webMatches.FirstOrDefault() ?? new WebServiceBook { Id = db.Id }
                }).ToList();

            return books;
        }

        public async Task CreateAsync(Book book) {
            var transaction = repository.BeginTransaction();

            try {
                DbBook? dbBook = await repository.CreateAsync(new DbBook {
                    AuthorId = book.AuthorId,
                });

                if (dbBook == null)
                    throw new Exception("Book not created");

                if (book.Metadata != null) {
                    await webService.CreateBookAsync(new WebServiceBook {
                        Id = dbBook.Id,
                        Title = book.Metadata.Title,
                        Description = book.Metadata.Description,
                        ImageUrl = book.Metadata.ImageUrl,
                        Chapters = book.Metadata.Chapters
                    });
                }

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Book book) {
            if (book.Metadata == null)
                return;

            await webService.UpdateBookAsync(book.Metadata);
        }

        public async Task DeleteAsync(int id) {
            var transaction = repository.BeginTransaction();

            try {
                await repository.DeleteAsync(id);
                await webService.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}