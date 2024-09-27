using BusinessLogicLayer.Events;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Initiate {
    public static class EventSubscriber {
        public static void Subscribe(IServiceProvider serviceProvider) {
            UserEvents.BookAddedToLibrary += async (sender, args) => {
                using var scope = serviceProvider.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();    

                var book = await bookService.GetBookAsync(args.BookId);
                var library = await libraryService.GetLibraryAsync(args.LibraryId);

                if (book != null && library != null) {
                    await notificationService.CreateAsync(book.AuthorId,
                        $"Your book '{book.Title}' has been added to the library '{library.Name}'.");
                }
            };


            UserEvents.UserJoinedLibrary += async (sender, args) => {
                using var scope = serviceProvider.CreateScope();
                INotificationService notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                ILibraryService libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
                IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                ILibraryMembershipService libraryMembershipService = scope.ServiceProvider.GetRequiredService<ILibraryMembershipService>();

                Library library = await libraryService.GetLibraryAsync(args.LibraryId);
                User user = await userService.GetByIdAsync(args.UserId);
                var managers = libraryMembershipService.GetLibraryMembers(library.Id).Where(member => member.Role == MembershipRole.Manager);

                string message = $"The user '{user.UserName}' has joined to the library '{library.Name}'";

                foreach (var manager in managers) 
                    await notificationService.CreateAsync(manager.UserId, message);
            };
        }
    }
}