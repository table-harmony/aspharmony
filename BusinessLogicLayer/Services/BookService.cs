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

    public class BookService(IBookRepository repository, Func<int, Task<IBookServer>> serverFactory) : IBookService {

        private async Task<IBookServer> GetServerAsync(int serverId) => await serverFactory(serverId);

        public async Task<Book?> GetBookAsync(int id) {
            DbBook? dbBook = await repository.GetBookAsync(id);
            if (dbBook == null) return null;

            IBookServer server = await GetServerAsync(dbBook.ServerId);

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

            if (!dbBooks.Any())
                return [];

            var server = await GetServerAsync(dbBooks.First().ServerId);

            List<ServerBook> webBooks = await server.GetAllBooksAsync();

            var books = dbBooks.GroupJoin(webBooks,
                db => db.Id,
                web => web.Id,
                (db, webMatches) => new Book {
                    Id = db.Id,
                    Server = db.Server,
                    ServerId = db.ServerId,
                    Author = db.Author,
                    AuthorId = db.AuthorId,
                    Metadata = webMatches.FirstOrDefault() ?? new ServerBook { Id = db.Id }
                }).ToList();

            return books;
        }

        public async Task CreateAsync(Book book) {
            var transaction = repository.BeginTransaction();

            try {
                DbBook dbBook = await repository.CreateAsync(new DbBook {
                    AuthorId = book.AuthorId,
                    ServerId = book.ServerId,
                }) ?? throw new Exception("Book not created");

                if (book.Metadata != null) {
                    var server = await GetServerAsync(dbBook.ServerId);

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

            var server = await GetServerAsync(book.ServerId);
            await server.UpdateBookAsync(book.Metadata);
        }

        public async Task DeleteAsync(int id) {
            var transaction = repository.BeginTransaction();

            try {
                var book = await repository.GetBookAsync(id);

                if (book == null)
                    return;

                await repository.DeleteAsync(id);

                var server = await GetServerAsync(book.ServerId);
                await server.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}