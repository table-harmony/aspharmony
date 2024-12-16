using AspClient.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AspClient.Controllers {
    public class LibraryController {
        private readonly LibraryService _libraryService = new LibraryService();

        public async Task<List<LibraryResponse>> GetAllAsync() {
            return await _libraryService.GetAllAsync();
        }

        public async Task<LibraryResponse> GetLibraryAsync(int id) {
            return await _libraryService.GetLibraryAsync(id);
        }

        public async Task<LibraryResponse> CreateAsync(string name, bool allowCopies) {
            return await _libraryService.CreateAsync(name, allowCopies);
        }

        public async Task JoinAsync(int libraryId, string userId, string role) {
            await _libraryService.JoinAsync(libraryId, userId, role);
        }

        public async Task UpdateAsync(int id, string name, bool allowCopies) {
            await _libraryService.UpdateAsync(id, name, allowCopies);
        }

        public async Task DeleteAsync(int id) {
            await _libraryService.DeleteAsync(id);
        }

        public async Task AddBookAsync(int libraryId, int bookId) {
            await _libraryService.AddBookAsync(libraryId, bookId);
        }

        public async Task RemoveBookAsync(int libraryId, int bookId) {
            await _libraryService.RemoveBookAsync(libraryId, bookId);
        }
    }
}