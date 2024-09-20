using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Encryption;

namespace BusinessLogicLayer.Services {
    public class BookService : IBookService {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public Task CreateAsync() {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id) {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task<Book> GetByIdAsync(int id) {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Book book) {
            throw new NotImplementedException();

        }
    }
}
