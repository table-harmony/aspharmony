using BusinessLogicLayer.Events;
using BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer.Initiate {
    public static class EventSubscriber {
        public static void Subscribe(IServiceProvider serviceProvider) {
            //UserEvents.UserLoggedIn += async (sender, args) => {
            //    using var scope = serviceProvider.CreateScope();
            //    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            //    await notificationService.CreateNotificationAsync(args.UserId, "You have successfully logged in.");
            //};

            //UserEvents.BookAddedToLibrary += async (sender, args) => {
            //    using var scope = serviceProvider.CreateScope();
            //    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            //    await notificationService.CreateNotificationAsync(args.AuthorId, $"Your book '{args.BookTitle}' has been added to the library '{args.LibraryName}'.");
            //};

            //BookLoanEvents.BookBorrowed += async (sender, args) => {
            //    using var scope = serviceProvider.CreateScope();
            //    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            //    var libraryService = scope.ServiceProvider.GetRequiredService<ILibraryService>();
            //    var managers = await libraryService.GetLibraryManagersAsync(args.LibraryId);
            //    foreach (var manager in managers) {
            //        if (manager != null)
            //            await notificationService.CreateNotificationAsync(manager.Id, $"Book '{args.BookTitle}' has been borrowed by user {args.UserId}.");
            //    }
            //};
        }
    }
}