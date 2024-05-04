using aspharmony.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace aspharmony.Controller {
    public class ControllerUser {

        public static DataTable GetUsers() {
            return ModelUsers.GetUsers();
        }

        public static DataTable GetUserByUsername(string username)  {
            return ModelUsers.GetUserByUsername(username);
        }

        public static bool IsUserExist(string username, string password) {
            DataTable dt = ModelUsers.GetUserByCredentials(username, password);
            return dt.Rows.Count != 0;
        }

        public static bool IsUserExist(string username) {
            DataTable dt = ModelUsers.GetUserByUsername(username);
            return dt.Rows.Count != 0;
        }

        public static bool CreateUser(string username, string password, string gmail, int gender) {
            if (IsUserExist(username))
                return false;
            ModelUsers.CreateUser(username, password, gmail, gender);
            return true;
        }

        public static bool DeleteUserByUsername(string username) {
            if (!IsUserExist(username))
                return false;
            ModelUsers.DeleteUserByUsername(username);
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