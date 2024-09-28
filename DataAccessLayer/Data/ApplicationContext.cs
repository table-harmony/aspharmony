using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace DataAccessLayer.Data {
    public class ApplicationContext : IdentityDbContext<User> {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasMany(user => user.Books)
                .WithOne(book => book.Author)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Memberships)
                .WithOne(membership => membership.User)
                .HasForeignKey(membership => membership.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Library
            modelBuilder.Entity<Library>()
                .HasMany(l => l.Memberships)
                .WithOne(m => m.Library)
                .HasForeignKey(m => m.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Library>()
                .HasMany(l => l.Books)
                .WithOne(lb => lb.Library)
                .HasForeignKey(lb => lb.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            // LibraryMembership
            modelBuilder.Entity<LibraryMembership>()
                .Property(m => m.Role)
                .HasConversion<string>();

            modelBuilder.Entity<LibraryMembership>()
                .HasOne(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryMembership>()
                .HasOne(m => m.Library)
                .WithMany(l => l.Memberships)
                .HasForeignKey(m => m.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            // LibraryBook
            modelBuilder.Entity<LibraryBook>()
                .HasOne(lb => lb.Library)
                .WithMany(l => l.Books)
                .HasForeignKey(lb => lb.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryBook>()
                .HasOne(lb => lb.Book)
                .WithMany(b => b.LibraryBooks)
                .HasForeignKey(lb => lb.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LibraryBook>()
                .HasMany(lb => lb.Loans)
                .WithOne(bl => bl.LibraryBook)
                .HasForeignKey(bl => bl.LibraryBookId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookLoan
            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.LibraryBook)
                .WithMany(lb => lb.Loans)
                .HasForeignKey(bl => bl.LibraryBookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookLoan>()
                .HasOne(bl => bl.LibraryMembership)
                .WithMany(m => m.BookLoans)
                .HasForeignKey(bl => bl.LibraryMembershipId)
                .OnDelete(DeleteBehavior.NoAction);

            // Notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryBook> LibraryBooks { get; set; }
        public DbSet<LibraryMembership> LibraryMemberships { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (Database.CurrentTransaction != null)
            {
                return null;
            }

            return await Database.BeginTransactionAsync();
        }
    }
}
