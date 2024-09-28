using DataAccessLayer.Entities;

using Book = BusinessLogicLayer.Services.Book;

namespace BusinessLogicLayer.Events
{
    public interface IEventPublisher {
        // User events
        Task PublishUserRegistered(User user);
        Task PublishUserUpdated(User user);
        Task PublishUserDeleted(User user);
        Task PublishUserLoggedIn(User user);

        // Library events
        void PublishUserJoinedLibrary(LibraryMembership membership);
        void PublishUserLeftLibrary(LibraryMembership membership);
        void PublishBookAddedToLibrary(LibraryBook libraryBook);
        void PublishBookRemovedFromLibrary(LibraryBook libraryBook);
    }

    public class EventPublisher : IEventPublisher {
        // User events
        public async Task PublishUserRegistered(User user) {
            await UserEvents.OnUserRegistered(user);
        }

        public async Task PublishUserUpdated(User user) {
            await UserEvents.OnUserUpdated(user);
        }

        public async Task PublishUserDeleted(User user) {
            await UserEvents.OnUserDeleted(user);
        }

        public async Task PublishUserLoggedIn(User user) {
            await UserEvents.OnUserLoggedIn(user);
        }

        // Library events
        public void PublishUserJoinedLibrary(LibraryMembership membership) {
            LibraryEvents.OnUserJoinedLibrary(membership);
        }

        public void PublishUserLeftLibrary(LibraryMembership membership) {
            LibraryEvents.OnUserLeftLibrary(membership);
        }

        public void PublishBookAddedToLibrary(LibraryBook libraryBook) {
            LibraryEvents.OnBookAddedToLibrary(libraryBook);
        }

        public void PublishBookRemovedFromLibrary(LibraryBook libraryBook) {
            LibraryEvents.OnBookRemovedFromLibrary(libraryBook);
        }
    }
}