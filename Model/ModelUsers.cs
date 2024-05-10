using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace aspharmony.Model
{
    public class ModelUsers {

        public static DataTable GetUsers() {
            DataTable foundUsers = AdoHelper.GetDataTable("getUsers", true);
            return foundUsers;
        }

        public static DataTable GetUser(int id) {
            DataTable foundUser = AdoHelper.GetDataTable("getUser", new { id }, true);

            if (foundUser.Rows.Count == 0)
                throw new Exception("User not found!");

            return foundUser;
        }

        public static DataTable GetUserByEmail(string email) {
            DataTable foundUser = AdoHelper.GetDataTable("SELECT * FROM users WHERE email = @email", new { email });
            return foundUser;
        }

        public static void CreateUser(string email, string password,
                                      int gender, int role = 1) {
            AdoHelper.GetDataTable("INSERT INTO users (email, password, gender, role)" +
                "VALUES (@email, @password, @gender, @role)",
                new { email, password, gender, role });
        }

        public static void DeleteUser(int id) {
            AdoHelper.GetDataTable("deleteUser", new { id }, true);
        }

        public static void UpdateUser(int id, string email, 
                                      string password, int gender, int role) {
            AdoHelper.GetDataTable("UPDATE users SET email = @email, " +
                "password = @password, gender = @gender, role = @role WHERE id = @id",
                new { id, email, password, gender, role });
        }

    }
}