using BusinessLogicLayer.Events;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using Utils.Services;

namespace BusinessLogicLayer.Initiate {

    public static class EventSubscriber {
        private static IServiceProvider _serviceProvider;

        public static void Subscribe(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;

            InitializeUserEvents(serviceProvider);

            SubscribeUserEvents();
            SubscribeLibraryEvents();
        }

        private static void InitializeUserEvents(IServiceProvider serviceProvider) {
            var devHarmonyApiService = serviceProvider.GetRequiredService<IDevHarmonyApiService>();
            UserEvents.Initialize(devHarmonyApiService);
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
                CreateNotification(args.User.Id, $"You have joined the library '{args.Library.Name}'.");
                NotifyLibraryManagers(args.Library.Id, 
                    $"User '{args.User.UserName}' has joined the library '{args.Library.Name}'.");
            };

            LibraryEvents.UserLeftLibrary += (sender, args) =>
                CreateNotification(args.User.Id, $"You have left the library '{args.Library.Name}'.");

            LibraryEvents.BookAddedToLibrary += (sender, args) =>
                NotifyLibraryMembers(args.Library.Id, 
                    $"New book '{args.BookTitle}' has been added to the library '{args.Library.Name}'.");

            LibraryEvents.BookRemovedFromLibrary += (sender, args) =>
                NotifyLibraryManagers(args.Library.Id, 
                    $"Book '{args.BookTitle}' has been removed from the library '{args.Library.Name}'.");
        }

        private static void CreateNotification(string userId, string message) {
            using var scope = _serviceProvider.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            
            notificationService.CreateAsync(userId, message).Wait();
        }

        private static void NotifyLibraryMembers(int libraryId, string message) {
            using var scope = _serviceProvider.CreateScope();
            var libraryMembershipService = scope.ServiceProvider.GetRequiredService<ILibraryMembershipService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var members = libraryMembershipService.GetLibraryMembers(libraryId);
            foreach (var member in members) {
                notificationService.CreateAsync(member.UserId, message).Wait();
            }
        }

        private static void NotifyLibraryManagers(int libraryId, string message) {
            using var scope = _serviceProvider.CreateScope();
            var libraryMembershipService = scope.ServiceProvider.GetRequiredService<ILibraryMembershipService>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var managers = libraryMembershipService.GetLibraryMembers(libraryId).Where(member => member.Role == MembershipRole.Manager);
            foreach (var manager in managers) {
                notificationService.CreateAsync(manager.UserId, message).Wait();
            }
        }
    }
}