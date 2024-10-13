using DataAccessLayer.Repositories;
using BusinessLogicLayer.Servers.Books;

using DbBook = DataAccessLayer.Entities.Book;
using ServerBook = BusinessLogicLayer.Servers.Books.Book;

namespace BusinessLogicLayer.Services {
    public class Book : DbBook {
        public ServerBook? Metadata { get; set; }
    }

    public interface IBookService {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService(IBookRepository repository, IBookServer server) : IBookService {
        
        public async Task<Book?> GetBookAsync(int id) {
            DbBook? dbBook = await repository.GetBookAsync(id);
            if (dbBook == null) return null;

            ServerBook? webBook = await server.GetBookAsync(id);

            return new Book {
                Id = dbBook.Id,
                Author = dbBook.Author,
                AuthorId = dbBook.AuthorId,
                Metadata = webBook
            };
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            IEnumerable<DbBook> dbBooks = await repository.GetAllAsync();
            List<ServerBook> webBooks = await server.GetAllBooksAsync();

            var books = dbBooks.GroupJoin(webBooks,
                db => db.Id,
                web => web.Id,
                (db, webMatches) => new Book {
                    Id = db.Id,
                    Author = db.Author,
                    AuthorId = db.AuthorId,
                    Metadata = webMatches.FirstOrDefault() ?? new ServerBook { Id = db.Id }
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
                    await server.CreateBookAsync(new ServerBook {
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

            await server.UpdateBookAsync(book.Metadata);
        }

        public async Task DeleteAsync(int id) {
            var transaction = repository.BeginTransaction();

            try {
                await repository.DeleteAsync(id);
                await server.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}