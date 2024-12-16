using AspClient.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AspClient.Controllers {
    public class BookController {
        private readonly BookService _bookService = new BookService();

        public async Task<List<BookResponse>> GetAllAsync(string serverType = null) {
            return await _bookService.GetAllAsync(serverType);
        }

        public async Task<BookResponse> GetBookAsync(int id) {
            return await _bookService.GetBookAsync(id);
        }

        public async Task<BookResponse> CreateAsync(
            string title,
            string description,
            string imageUrl,
            int server,
            string authorId) {
            return await _bookService.CreateAsync(
                title,
                description,
                imageUrl,
                server,
                authorId
            );
        }

        public async Task UpdateAsync(
            int id,
            string authorId,
            int server,
            string title,
            string description,
            string imageUrl) {
            await _bookService.UpdateAsync(
                id,
                authorId,
                server,
                title,
                description,
                imageUrl
            );
        }

        public async Task DeleteAsync(int id) {
            await _bookService.DeleteAsync(id);
        }
    }
}