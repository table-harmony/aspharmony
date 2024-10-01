using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IFeedbackRepository {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task CreateAsync(Feedback feedback);
        Task DeleteAsync(int id);
    }

    public class FeedbackRepository : IFeedbackRepository {
        private readonly ApplicationContext _context;

        public FeedbackRepository(ApplicationContext context) {
            _context = context;
        }

        public async Task<Feedback> GetAsync(int id) {
            return await _context.Feedbacks
                .Include(b => b.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync() {
            return await _context.Feedbacks
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task CreateAsync(Feedback feedback) {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Feedback feedback = await GetAsync(id);

            if (feedback == null)
                throw new NotFoundException();

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }
}