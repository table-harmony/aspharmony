using DataAccessLayer.Entities;
using Utils.Services;

namespace BusinessLogicLayer.Events {
    public class UserEventArgs : EventArgs {
        public required User User { get; set; }
    }

    public static class UserEvents {
        private static IEventTracker _eventTracker;

        public static void Initialize(IEventTracker eventTracker) {
            _eventTracker = eventTracker;
        }

        // Multicast delegate for user-related events
        public delegate void UserEventHandler(object sender, UserEventArgs e);

        // Events
        public static event UserEventHandler UserRegistered;
        public static event UserEventHandler UserUpdated;
        public static event UserEventHandler UserDeleted;
        public static event UserEventHandler UserLoggedIn;

        // Event raising methods
        public static async Task OnUserRegistered(User user) {
            UserRegistered?.Invoke(null, new UserEventArgs { User = user });
            await TrackEventAsync("User registered");
        }

        public static async Task OnUserUpdated(User user) {
            UserUpdated?.Invoke(null, new UserEventArgs { User = user });
            await TrackEventAsync("User updated");
        }

        public static async Task OnUserDeleted(User user) {
            UserDeleted?.Invoke(null, new UserEventArgs { User = user });
            await TrackEventAsync("User deleted");
        }

        public static async Task OnUserLoggedIn(User user) {
            UserLoggedIn?.Invoke(null, new UserEventArgs { User = user });
            await TrackEventAsync("User logged in");
        }

        private static async Task TrackEventAsync(string eventKey) {
            await _eventTracker.TrackEventAsync(eventKey);
        }
    }
}
