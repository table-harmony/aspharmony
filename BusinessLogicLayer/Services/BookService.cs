using BookServiceReference;
using DataAccessLayer.Repositories;

using DbBook = DataAccessLayer.Entities.Book;
using SoapBook = BookServiceReference.Book;
using Chapter = BookServiceReference.Chapter;

namespace BusinessLogicLayer.Services {
    public class Book : DbBook {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    }

    public interface IBookService {
        Task<Book> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task CreateAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
    }

    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;
        private readonly BookServiceSoapClient _soapClient;

        public BookService(IBookRepository bookRepository, BookServiceSoapClient soapClient) {
            _bookRepository = bookRepository;
            _soapClient = soapClient;
        }

        public async Task<Book> GetBookAsync(int id) {
            var dbBook = await _bookRepository.GetBookAsync(id);
            if (dbBook == null) return null;

            var soapBook = (await _soapClient.GetBookAsync(id)).Body.GetBookResult;

            Book book = new() {
                Id = dbBook.Id,
                Title = soapBook.Title,
                Description = soapBook.Description,
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
            var dbBooks = await _bookRepository.GetAllAsync();
            var soapBooks = (await _soapClient.GetAllBooksAsync()).Body.GetAllBooksResult;

            List<Book> books = new(); 

            foreach (var dbBook in dbBooks) {
                var soapBook = soapBooks.FirstOrDefault(b => b.Id == dbBook.Id);
                if (soapBook != null) {
                    books.Add(new() {
                        Id = dbBook.Id,
                        Title = soapBook.Title,
                        Description = soapBook.Description,
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
            var transaction = _bookRepository.BeginTransaction();

            try {
                var dbBook = await _bookRepository.CreateAsync(new DbBook {
                    AuthorId = book.AuthorId,
                });

                await _soapClient.CreateBookAsync(new SoapBook {
                    Id = dbBook.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Chapters = book.Chapters.Select((c, index) => new BookServiceReference.Chapter {
                        Index = index,
                        Title = c.Title,
                        Content = c.Content
                    }).ToArray()
                });

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
            }
        }

        public async Task UpdateAsync(Book book) {
            await _soapClient.UpdateBookAsync(new SoapBook {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Chapters = book.Chapters.Select(c => new BookServiceReference.Chapter {
                    Index = c.Index,
                    Title = c.Title,
                    Content = c.Content
                }).ToArray()
            });
        }

        public async Task DeleteAsync(int id) {
            var transaction = _bookRepository.BeginTransaction();

            try {
                await _bookRepository.DeleteAsync(id);
                await _soapClient.DeleteBookAsync(id);

                transaction.Commit();
            } catch {
                await transaction.RollbackAsync();
            }
        }
    }
}
