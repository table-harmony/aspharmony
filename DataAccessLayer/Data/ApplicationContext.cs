using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;    

namespace DataAccessLayer.Data {
        public class ApplicationContext : IdentityDbContext<User> {
            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
                base.OnModelCreating(modelBuilder);

                // User
                modelBuilder.Entity<User>()
                    .HasMany(u => u.Books)
                    .WithOne(b => b.Author)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<User>()
                    .HasMany(u => u.Memberships)
                    .WithOne(m => m.User)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<User>()
                    .HasMany(u => u.BookLoans)
                    .WithOne(bl => bl.User)
                    .HasForeignKey(bl => bl.UserId)
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
                    .HasKey(m => new { m.UserId, m.LibraryId });

                modelBuilder.Entity<LibraryMembership>()
                    .Property(m => m.Role)
                    .HasConversion<string>();

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
                    .HasOne(bl => bl.Library)
                    .WithMany()
                    .HasForeignKey(bl => bl.LibraryId)
                    .OnDelete(DeleteBehavior.Restrict);

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
        }
    }
