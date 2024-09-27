using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Events
{
    public interface IEventPublisher {
        // User events
        Task PublishUserRegistered(User user);
        Task PublishUserUpdated(User user);
        Task PublishUserDeleted(User user);
        Task PublishUserLoggedIn(User user);

        // Library events
        void PublishUserJoinedLibrary(User user, Library library);
        void PublishUserLeftLibrary(User user, Library library);
        void PublishBookAddedToLibrary(Book book, Library library, string bookTitle);
        void PublishBookRemovedFromLibrary(Book book, Library library, string bookTitle);
    }

    public class EventPublisher : IEventPublisher
    {
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
        public void PublishUserJoinedLibrary(User user, Library library) {
            LibraryEvents.OnUserJoinedLibrary(user, library);
        }

        public void PublishUserLeftLibrary(User user, Library library) {
            LibraryEvents.OnUserLeftLibrary(user, library);
        }

        public void PublishBookAddedToLibrary(Book book, Library library, string bookTitle) {
            LibraryEvents.OnBookAddedToLibrary(book, library, bookTitle);
        }

        public void PublishBookRemovedFromLibrary(Book book, Library library, string bookTitle) {
            LibraryEvents.OnBookRemovedFromLibrary(book, library, bookTitle);
        }
    }
}