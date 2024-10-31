using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Utils;
using Book = BusinessLogicLayer.Services.Book;

namespace BusinessLogicLayer.Events {

    public static class EventSubscriber {
        private static IServiceProvider? _serviceProvider;

        public static void Subscribe(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;

            InitializeUserEvents();

            SubscribeUserEvents();
            SubscribeLibraryEvents();
        }

        private static void InitializeUserEvents()  {
            if (_serviceProvider == null)
                return;

            var eventTracker = _serviceProvider.GetRequiredService<IEventTracker>();
            UserEvents.Initialize(eventTracker);
        }

        private static void SubscribeUserEvents() {
            UserEvents.UserRegistered += (sender, args) =>
                CreateNotification(args.User.Id, $"Welcome to AspHarmony, {args.User.UserName}!");

            UserEvents.UserUpdated += (sender, args) =>
                CreateNotification(args.User.Id, "Your account has been updated.");

            UserEvents.UserDeleted += (sender, args) =>
                CreateNotification(args.User.Id, "Your account has been deleted.");

            UserEvents.UserLoggedIn += (sender, args) =>
                CreateNotification(args.User.Id, $"Welcome back, {args.User.UserName}!");
        }

        private static void SubscribeLibraryEvents() {
            LibraryEvents.UserJoinedLibrary += (sender, args) => {
                LibraryMembership membership = args.Membership;

                CreateNotification(membership.UserId, $"You have joined the library '{membership.Library.Name}'.");
                NotifyLibraryMembers(membership.LibraryId,
                    $"User '{membership.User.UserName}' has joined the library '{membership.Library.Name}'.");
            };

            LibraryEvents.UserLeftLibrary += (sender, args) => {
                LibraryMembership membership = args.Membership;

                CreateNotification(membership.UserId, $"You have left the library '{membership.Library.Name}'.");
                NotifyLibraryMembers(membership.LibraryId,
                    $"User '{membership.User.UserName}' has left the library '{membership.Library.Name}'.");
            };

            LibraryEvents.BookAddedToLibrary += (sender, args) => {
                LibraryBook libraryBook = args.LibraryBook;
                NotifyLibraryMembers(libraryBook.LibraryId,
                    $"The book '{(libraryBook.Book as Book)?.Metadata?.Title}' has been added to the library '{libraryBook.Library.Name}'.");
            };

            LibraryEvents.BookRemovedFromLibrary += (sender, args) => {
                LibraryBook libraryBook = args.LibraryBook;
                NotifyLibraryMembers(libraryBook.LibraryId,
                    $"The book '{(libraryBook.Book as Book)?.Metadata?.Title}' has been removed from the library '{libraryBook.Library.Name}'.");
            };
        }

        private static void CreateNotification(string userId, string message) {
            try {
                if (_serviceProvider == null)
                    return;

                using var scope = _serviceProvider.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                notificationService.CreateAsync(userId, message).Wait();
            } catch {
                throw;
            }
        }

        private static void NotifyLibraryMembers(int libraryId, string message) {
            if (_serviceProvider == null)
                return;

            using var scope = _serviceProvider.CreateScope();
            var libraryMembershipService = scope.ServiceProvider.GetRequiredService<ILibraryMembershipService>();

            libraryMembershipService.GetLibraryMembers(libraryId)
                .ToList()
                .ForEach(member => {
                    CreateNotification(member.User.Id, message);
                });
        }
    }
}