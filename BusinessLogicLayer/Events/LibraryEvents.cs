using System;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Events
{
    public class LibraryMembershipEventArgs : EventArgs {
        public User User { get; set; }
        public Library Library { get; set; }
    }

    public class LibraryBookEventArgs : EventArgs {
        public Book Book { get; set; }
        public Library Library { get; set; }
        public string BookTitle { get; set; }
    }

    public static class LibraryEvents {

        // Events
        public static event EventHandler<LibraryMembershipEventArgs> UserJoinedLibrary;
        public static event EventHandler<LibraryMembershipEventArgs> UserLeftLibrary;
        public static event EventHandler<LibraryBookEventArgs> BookAddedToLibrary;
        public static event EventHandler<LibraryBookEventArgs> BookRemovedFromLibrary;

        // Event raising methods
        public static void OnUserJoinedLibrary(User user, Library library) {
            UserJoinedLibrary?.Invoke(null, new LibraryMembershipEventArgs { User = user, Library = library });
        }

        public static void OnUserLeftLibrary(User user, Library library) {
            UserLeftLibrary?.Invoke(null, new LibraryMembershipEventArgs { User = user, Library = library });
        }

        public static void OnBookAddedToLibrary(Book book, Library library, string bookTitle) {
            BookAddedToLibrary?.Invoke(null, new LibraryBookEventArgs { Book = book, Library = library, BookTitle = bookTitle });
        }

        public static void OnBookRemovedFromLibrary(Book book, Library library, string bookTitle) {
            BookRemovedFromLibrary?.Invoke(null, new LibraryBookEventArgs { Book = book, Library = library, BookTitle = bookTitle });
        }
    }
}