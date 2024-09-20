using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
        }

        public DbSet<User> Users { get; set; }
    }
}
