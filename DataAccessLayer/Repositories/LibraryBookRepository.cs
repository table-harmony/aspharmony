using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories {
    public interface ILibraryBookRepository {
        LibraryBook GetBook(int id);
        LibraryBook GetBook(int libraryId, int bookId);
        Task CreateAsync(LibraryBook book);
        IEnumerable<LibraryBook> GetLibraryBooks(int libraryId);
        Task DeleteAsync(int id);
    }

    public class LibraryBookRepository : ILibraryBookRepository {
        private readonly ApplicationContext _context;

        public LibraryBookRepository(ApplicationContext context) {
            _context = context;
        }

        public LibraryBook GetBook(int id) {
            return _context.LibraryBooks
                .Include(libraryBook => libraryBook.Library)
                .Include(libraryBook => libraryBook.Book)
                .First(libraryBook => libraryBook.Id == id);
        }

        public LibraryBook GetBook(int libraryId, int bookId) {
            return _context.LibraryBooks
                .Include(libraryBook => libraryBook.Library)
                .Include(libraryBook => libraryBook.Book)
                .First(libraryBook => libraryBook.Book.Id == bookId && 
                                                    libraryBook.Library.Id == libraryId);
        }

        public async Task<IEnumerable<LibraryBook>> GetLibraryBooks(int libraryId) {
            return await _context.LibraryBooks
                .Include(lb => lb.Book)
                .Where(lb => lb.LibraryId == libraryId)
                .ToListAsync();
        }

        public async Task<LibraryBook> GetLibraryBookByIdAsync(int id) {
            return await _context.LibraryBooks
                .Include(lb => lb.Book)
                .FirstOrDefaultAsync(lb => lb.Id == id);
        }

        public async Task DeleteAsync(int id) {
            var libraryBook = await _context.LibraryBooks.FindAsync(id);
            if (libraryBook != null) {
                _context.LibraryBooks.Remove(libraryBook);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(LibraryBook book) {
            await _context.LibraryBooks.AddAsync(book);
            await _context.SaveChangesAsync();
        }
    }
}
