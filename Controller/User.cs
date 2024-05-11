using aspharmony.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace aspharmony.Controller {

    public class ControllerUser {

        public static DataTable GetUsers() {
            return ModelUsers.GetUsers();
        }

        public static DataTable GetUser(int id) {
            DataTable foundUser = ModelUsers.GetUser(id);

            if (foundUser.Rows.Count == 0)
                throw new Exception("User not found!");

            return foundUser;
        }

        public static DataTable GetUserByEmail(string email) {
            return ModelUsers.GetUserByEmail(email);
        }

        public static DataTable GetUserByCredentials(string email, string password) {
            DataTable foundUser = GetUserByEmail(email);

            if (foundUser.Rows.Count == 0)
                throw new Exception("User not found!");

            if (password != foundUser.Rows[0]["password"].ToString())
                throw new Exception("Invalid Credentials!");

            return foundUser;
        }

        public static void CreateUser(string email, string password, string name) {
            DataTable existingUser = GetUserByEmail(email);

            if (existingUser.Rows.Count != 0)
                throw new Exception("User already exists!");

            ModelUsers.CreateUser(email, password, name);
        }

        public static void DeleteUser(int id) {
            ModelUsers.DeleteUser(id);
        }

        public static void UpdateUser(int id, string email, string password, string name, int role) {
            DataTable updatedUser = GetUser(id);
            DataTable existingUser = GetUserByEmail(email);

            if (existingUser.Rows.Count != 0 &&
                existingUser.Rows[0]["id"].ToString() != updatedUser.Rows[0]["id"].ToString())
                throw new Exception("User already exists with the same email!");

            ModelUsers.UpdateUser(id, email, password, name, role);
        }
    }
}