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

        public static DataTable GetUser(int id) {
            return AdoHelper.GetDataTable("getUser", new { id }, true);
        }

        public static DataTable GetUserByEmail(string email) {
            return AdoHelper.GetDataTable("SELECT * FROM users WHERE email = @email", 
                                            new { email });
        }

        public static void CreateUser(string email, string password,
                                      string name, int role = 1) {
            AdoHelper.GetDataTable("INSERT INTO users (email, password, name, role)" +
                "VALUES (@email, @password, @name, @role)",
                new { email, password, name, role });
        }

        public static void DeleteUser(int id) {
            AdoHelper.GetDataTable("deleteUser", new { id }, true);
        }

        public static void UpdateUser(int id, string email, 
                                      string password, string name, int role) {
            AdoHelper.GetDataTable("UPDATE users SET email = @email, " +
                "password = @password, name = @name, role = @role WHERE id = @id",
                new { id, email, password, name, role });
        }

    }
}