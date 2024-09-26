using BusinessLogicLayer.Events;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Initiate {
    public static class EventSubscriber {
        public static void Subscribe(IServiceProvider serviceProvider) {
            UserEvents.BookAddedToLibrary += async (sender, args) => {
                using var scope = serviceProvider.CreateScope();
                INotificationService notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                IBookService bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                ILibraryService libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();    

                Book book = await bookService.GetBookAsync(args.BookId);
                Library library = await libraryService.GetLibraryAsync(args.LibraryId);

                string message = $"Your book '{book.Title}' has been added to the library '{library.Name}'.";
                    
                await notificationService.CreateAsync(book.AuthorId, message);
            };

        }
    }
}