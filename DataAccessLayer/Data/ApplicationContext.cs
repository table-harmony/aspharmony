using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Entity>()
                    .Property(e => e.CreationTime)
                    .HasDefaultValueSql("GETDATE()");

            builder.Entity<User>()
                    .HasIndex(user => user.Email)
                    .IsUnique();
        }

        public DbSet<User> Users { get; set; }
    }
}
