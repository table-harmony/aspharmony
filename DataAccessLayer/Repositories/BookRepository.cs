using DataAccessLayer.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public class BookRepository : IBookRepository {
        private readonly ApplicationContext _context;

        public BookRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Book> GetByIdAsync(int id) {
            return await _context.Books.FindAsync(id);
        }

        public async Task CreateAsync(Book book) {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book) {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Book book = await GetByIdAsync(id);

            if (book == null)
                throw new NotFoundException();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }

}
