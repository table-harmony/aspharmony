using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace aspharmony.Model
{
    public class ModelUsers {

        public static DataTable GetUsers() {
            return AdoHelper.GetDataTable("getUsers", true);
        }

        public static DataTable GetUserByUsername(string username) {
            DataTable dt = AdoHelper.GetDataTable("getUserByUsername", new { username }, true);
            return dt;
        }

        public static DataTable GetUserByCredentials(string username, string password) {
            DataTable dt = AdoHelper.GetDataTable("getUserByCredentials", new { username, password }, true);
            return dt; 
        }

        public static void CreateUser(string username, string password, string gmail,
                                      int gender, int accesskey = 1) {
            AdoHelper.GetDataTable("INSERT INTO userTable (usernameField, passwordField, gmailField, genderField, accesskeyField)" +
                "VALUES (@username, @password, @gmail, @gender, @accesskey)",
                new { username, password, gmail, gender, accesskey });
        }

        public static void DeleteUserByUsername(string username) {
            AdoHelper.GetDataTable("deleteUserByUsername", new { username }, true);
        }

        public static void UpdateUser(string username, string password, string gmail,
                                      int gender) {
            AdoHelper.GetDataTable("UPDATE userTable SET passwordField = @password, " +
                "gmailField = @gmail, genderField = @gender WHERE usernameField = @username",
                new { username, password, gmail, gender });
        }

    }
}