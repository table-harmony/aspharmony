using System;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Events
{
    public class UserEventArgs : EventArgs {
        public string UserId { get; set; }
    }

    public class LibraryMembershipEventArgs : EventArgs {
        public string UserId { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
    }

    public class BookAddedToLibraryEventArgs : EventArgs {
        public string AuthorId { get; set; }
        public string BookTitle { get; set; }
        public int LibraryId { get; set; }
        public string LibraryName { get; set; }
    }

    public static class UserEvents {
        public static event EventHandler<UserEventArgs> UserLoggedIn;
        public static event EventHandler<LibraryMembershipEventArgs> UserJoinedLibrary;
        public static event EventHandler<BookAddedToLibraryEventArgs> BookAddedToLibrary;

        public static void OnUserLoggedIn(string userId) {
            UserLoggedIn?.Invoke(null, new UserEventArgs { UserId = userId });
        }

        public static void OnUserJoinedLibrary(string userId, int libraryId, string libraryName) {
            UserJoinedLibrary?.Invoke(null, new LibraryMembershipEventArgs { UserId = userId, LibraryId = libraryId, LibraryName = libraryName });
        }

        public static void OnBookAddedToLibrary(string authorId, string bookTitle, int libraryId, string libraryName) {
            BookAddedToLibrary?.Invoke(null, new BookAddedToLibraryEventArgs { AuthorId = authorId, BookTitle = bookTitle, LibraryId = libraryId, LibraryName = libraryName });
        }
    }
}