using DataAccessLayer.Repositories;

using DbBook = DataAccessLayer.Entities.Book;
using ServerBook = BusinessLogicLayer.Servers.Books.Book;
using BusinessLogicLayer.Servers.Books;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Services
{
    public class Book : DbBook {
        public ServerBook? Metadata { get; set; }
    }

    public interface IBookService {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> GetAllAsync(ServerType serverType);
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService(IBookRepository repository, Func<ServerType, IBookServer> serverFactory) : IBookService {

        private IBookServer GetServer(ServerType serverType) => serverFactory(serverType);

        public async Task<Book?> GetBookAsync(int id) {
            DbBook? dbBook = await repository.GetBookAsync(id);
            if (dbBook == null) return null;

            var server = GetServer(dbBook.Server);

            ServerBook? webBook = await server.GetBookAsync(id);

            return new Book {
                Id = dbBook.Id,
                Server = dbBook.Server,
                Author = dbBook.Author,
                AuthorId = dbBook.AuthorId,
                Metadata = webBook
            };
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            var dbBooks = await repository.GetAllAsync();
            List<Book> books = [];

            foreach (var dbBook in dbBooks) {
                var server = GetServer(dbBook.Server);
                var webBook = await server.GetBookAsync(dbBook.Id);

                books.Add(new Book {
                    Id = dbBook.Id,
                    Server = dbBook.Server,
                    Author = dbBook.Author,
                    AuthorId = dbBook.AuthorId,
                    Metadata = webBook
                });
            }

            return books;
        }

        public async Task<IEnumerable<Book>> GetAllAsync(ServerType serverType) {
            var dbBooks = await repository.GetAllAsync(serverType);

            var server = GetServer(serverType);
            var webBooks = await server.GetAllBooksAsync();

            var books = dbBooks.Join(
                webBooks,
                db => db.Id,
                web => web.Id,
                (dbBook, webBook) => new Book {
                    Id = dbBook.Id,
                    Server = dbBook.Server,
                    Author = dbBook.Author,
                    AuthorId = dbBook.AuthorId,
                    Metadata = webBook
                }
            );

            return books.ToList();
        }

        public async Task CreateAsync(Book book) {
            var transaction = repository.BeginTransaction();

            try {
                DbBook dbBook = await repository.CreateAsync(new DbBook {
                    AuthorId = book.AuthorId,
                    Server = book.Server,
                }) ?? throw new Exception("Book not created");

                if (book.Metadata == null) {
                    transaction.Commit();
                    return;
                }

                var server = GetServer(dbBook.Server);

                await server.CreateBookAsync(new ServerBook {
                    Id = dbBook.Id,
                    Title = book.Metadata.Title,
                    Description = book.Metadata.Description,
                    ImageUrl = book.Metadata.ImageUrl,
                    Chapters = book.Metadata.Chapters
                });

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Book book) {
            if (book.Metadata == null)
                return;

            var server = GetServer(book.Server);
            await server.UpdateBookAsync(book.Metadata);
        }

        public async Task DeleteAsync(int id) {
            var transaction = repository.BeginTransaction();

            try {
                var book = await repository.GetBookAsync(id);

                if (book == null)
                    return;

                await repository.DeleteAsync(id);

                var server = GetServer(book.Server);
                await server.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}