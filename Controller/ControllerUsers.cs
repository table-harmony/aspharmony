using aspharmony.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace aspharmony.Controller {
    public class Controller_User {
        public static DataTable GetAllUsers() {
            return ModelUsers.GetAllUsers();
        }

        public static DataTable GetUserData(string username)  {
            return ModelUsers.GetUserData(username);
        }

        public static bool IsUserExist(string username, string password) {
            return ModelUsers.IsUserExist(username, password);
        }

        public static bool IsUserExist(string username) {
            return ModelUsers.IsUserExist(username);
        }

        public static bool CreateUser(string username, string password, string gmail, int gender) {
            if (IsUserExist(username))
                return false;
            ModelUsers.CreateUser(username, password, gmail, gender);
            return true;
        }

        public static bool DeleteUser(string username) {
            if (!IsUserExist(username))
                return false;
            ModelUsers.DeleteUser(username);
            return true;
        }

        public static bool UpdateUser(string username, string password, string gmail,
                                      int gender) {
            if (!IsUserExist(username))
                return false;
            ModelUsers.UpdateUser(username, password, gmail, gender);
            return true;
        }

    }
}