using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Events {
    public class UserEventArgs : EventArgs {
        public string UserId { get; set; }
    }

    public class LibraryMembershipEventArgs : EventArgs {
        public string UserId { get; set; }
        public int LibraryId { get; set; }
    }

    public class BookAddedToLibraryEventArgs : EventArgs {
        public int BookId { get; set; }
        public int LibraryId { get; set; }
    }

    public static class UserEvents {
        public static event EventHandler<LibraryMembershipEventArgs> UserJoinedLibrary;
        public static event EventHandler<BookAddedToLibraryEventArgs> BookAddedToLibrary;

        public static void OnUserJoinedLibrary(string userId, int libraryId) {
            UserJoinedLibrary?.Invoke(null, new LibraryMembershipEventArgs { UserId = userId, LibraryId = libraryId });
        }

        public static void OnBookAddedToLibrary(int bookId, int libraryId) {
            BookAddedToLibrary?.Invoke(null, new BookAddedToLibraryEventArgs { BookId = bookId, LibraryId = libraryId });
        }
    }
}
