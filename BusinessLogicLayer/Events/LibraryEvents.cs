using System;
using DataAccessLayer.Entities;

using Book = BusinessLogicLayer.Services.Book;

namespace BusinessLogicLayer.Events
{
    public class LibraryMembershipEventArgs : EventArgs {
        public LibraryMembership membership { get; set; }
    }

    public class LibraryBookEventArgs : EventArgs {
        public LibraryBook libraryBook { get; set; }
    }

    public static class LibraryEvents {

        // Events
        public static event EventHandler<LibraryMembershipEventArgs> UserJoinedLibrary;
        public static event EventHandler<LibraryMembershipEventArgs> UserLeftLibrary;
        public static event EventHandler<LibraryBookEventArgs> BookAddedToLibrary;
        public static event EventHandler<LibraryBookEventArgs> BookRemovedFromLibrary;

        // Event raising methods
        public static void OnUserJoinedLibrary(LibraryMembership membership) {
            UserJoinedLibrary?.Invoke(null, new LibraryMembershipEventArgs { membership = membership });
        }

        public static void OnUserLeftLibrary(LibraryMembership membership) {
            UserLeftLibrary?.Invoke(null, new LibraryMembershipEventArgs { membership = membership });
        }

        public static void OnBookAddedToLibrary(LibraryBook libraryBook) {
            BookAddedToLibrary?.Invoke(null, new LibraryBookEventArgs { libraryBook = libraryBook });
        }

        public static void OnBookRemovedFromLibrary(LibraryBook libraryBook) {
            BookRemovedFromLibrary?.Invoke(null, new LibraryBookEventArgs { libraryBook = libraryBook });
        }
    }
}