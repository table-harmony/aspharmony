using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data {
    public class ApplicationContext : IdentityDbContext<User> {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(user => user.Email)
                .IsUnique();

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Author)
                .WithMany(user => user.Books)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryMembership>()
                .Property(membership => membership.Role)
                .HasConversion<string>();

            modelBuilder.Entity<LibraryMembership>()
                .HasOne(membership => membership.User)
                .WithMany(user => user.Memberships)
                .HasForeignKey(membership => membership.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryMembership>()
                .HasOne(membership => membership.Library)
                .WithMany(library => library.Memberships)
                .HasForeignKey(membership => membership.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryBook>()
                .HasOne(lb => lb.Library)
                .WithMany(library => library.Books)
                .HasForeignKey(lb => lb.LibraryId);

            modelBuilder.Entity<LibraryBook>()
                .HasOne(lb => lb.Book)
                .WithMany()
                .HasForeignKey(lb => lb.BookId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryBook> LibraryBooks { get; set; }
        public DbSet<LibraryMembership> LibraryMemberships { get; set; }
    }
}
