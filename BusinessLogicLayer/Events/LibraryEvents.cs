using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Events {
    public class LibraryMembershipEventArgs : EventArgs {
        public required LibraryMembership Membership { get; set; }
    }

    public class LibraryBookEventArgs : EventArgs {
        public required LibraryBook LibraryBook { get; set; }
    }

    public static class LibraryEvents {

        public static event EventHandler<LibraryMembershipEventArgs>? UserJoinedLibrary;
        public static event EventHandler<LibraryMembershipEventArgs>? UserLeftLibrary;
        public static event EventHandler<LibraryBookEventArgs>? BookAddedToLibrary;
        public static event EventHandler<LibraryBookEventArgs>? BookRemovedFromLibrary;

        public static void OnUserJoinedLibrary(LibraryMembership membership) {
            UserJoinedLibrary?.Invoke(null, new LibraryMembershipEventArgs { Membership = membership });
        }

        public static void OnUserLeftLibrary(LibraryMembership membership) {
            UserLeftLibrary?.Invoke(null, new LibraryMembershipEventArgs { Membership = membership });
        }

        public static void OnBookAddedToLibrary(LibraryBook libraryBook) {
            BookAddedToLibrary?.Invoke(null, new LibraryBookEventArgs { LibraryBook = libraryBook });
        }

        public static void OnBookRemovedFromLibrary(LibraryBook libraryBook) {
            BookRemovedFromLibrary?.Invoke(null, new LibraryBookEventArgs { LibraryBook = libraryBook });
        }
    }
}