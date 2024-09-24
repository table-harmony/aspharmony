using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {
    public interface ILibraryBookService {
        LibraryBook GetBook(int id);
        LibraryBook GetBook(int libraryId, int bookId);
        Task CreateAsync(int libraryId, int bookId);
        IEnumerable<LibraryBook> GetLibraryBooks(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookService : ILibraryBookService {
        private readonly ILibraryBookRepository _libraryBookRepository;

        public LibraryBookService(ILibraryBookRepository libraryBookRepository) {
            _libraryBookRepository = libraryBookRepository;
        }

        public LibraryBook GetBook(int id) {
            return _libraryBookRepository.GetBook(id);
        }

        public LibraryBook GetBook(int libraryId, int bookId) {
            return _libraryBookRepository.GetBook(bookId, libraryId);
        }
        
        public IEnumerable<LibraryBook> GetLibraryBooks(int libraryId) {
            return _libraryBookRepository.GetLibraryBooks(libraryId);
        }

        public async Task CreateAsync(int libraryId, int bookId) {
            LibraryBook libraryBook = new() {
                LibraryId = libraryId,
                BookId = bookId,
            };

            await _libraryBookRepository.CreateAsync(libraryBook);
        }

        public async Task DeleteAsync(int id) {
            await _libraryBookRepository.DeleteAsync(id);
        }
    }
}
