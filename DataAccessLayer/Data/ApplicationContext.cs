using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccessLayer.Data {
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<User>(options) {
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            // User relationships
            builder.Entity<User>()
                .HasMany(user => user.Books)
                .WithOne(book => book.Author)
                .HasForeignKey(book => book.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(user => user.Memberships)
                .WithOne(membership => membership.User)
                .HasForeignKey(membership => membership.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(user => user.Notifications)
                .WithOne(notification => notification.User)
                .HasForeignKey(notification => notification.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(user => user.Senders)
                .WithOne(sender => sender.User)
                .HasForeignKey(sender => sender.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book relationships
            builder.Entity<Book>()
                .HasMany(book => book.AudioBooks)
                .WithOne(audioBook => audioBook.Book)
                .HasForeignKey(audioBook => audioBook.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Book>()
                .HasMany(book => book.LibraryBooks)
                .WithOne(libraryBook => libraryBook.Book)
                .HasForeignKey(libraryBook => libraryBook.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Library relationships
            builder.Entity<Library>()
                .HasMany(library => library.Memberships)
                .WithOne(membership => membership.Library)
                .HasForeignKey(membership => membership.LibraryId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Library>()
                .HasMany(library => library.Books)
                .WithOne(libraryBook => libraryBook.Library)
                .HasForeignKey(libraryBook => libraryBook.LibraryId)
                .OnDelete(DeleteBehavior.Cascade); 

            // BookLoan relationships
            builder.Entity<BookLoan>()
                .HasOne(bl => bl.LibraryBook)
                .WithMany(libraryBook => libraryBook.Loans)
                .HasForeignKey(bl => bl.LibraryBookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BookLoan>()
                .HasOne(bl => bl.LibraryMembership)
                .WithMany(membership => membership.BookLoans)
                .HasForeignKey(bl => bl.LibraryMembershipId)
                .OnDelete(DeleteBehavior.Restrict);
        }


        public DbSet<Sender> Senders { get; set; }
        public DbSet<UserSender> UserSenders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AudioBook> AudioBooks { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryBook> LibraryBooks { get; set; }
        public DbSet<LibraryMembership> LibraryMemberships { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}