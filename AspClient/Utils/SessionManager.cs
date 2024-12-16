using AspClient.Services;
using System.Web;
using System.Web.SessionState;

namespace AspClient.Utils {
    public static class SessionManager {
        private const string USER_KEY = "CurrentUser";
        
        private static HttpSessionState Session => HttpContext.Current?.Session;
        public static User CurrentUser => GetCurrentUser();


        public static void SetCurrentUser(User user) {
            if (Session == null)
                return;

            var sanitizedUser = new User {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
            };
            Session[USER_KEY] = sanitizedUser;
        }

        public static User GetCurrentUser() {
            return Session?[USER_KEY] as User;
        }

        public static void ClearCurrentUser() {
            Session?.Remove(USER_KEY);
        }

        public static bool IsUserLoggedIn() {
            return GetCurrentUser() != null;
        }
    }
}