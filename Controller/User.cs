using aspharmony.Entities;
using aspharmony.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace aspharmony.Controller {

    public class UserController {

        public static DataTable GetUsers() {
            return UserModel.GetUsers();
        }

        public static UserEntity GetUser(int id) {
            return UserModel.GetUser(id);
        }

        public static UserEntity GetUserByEmail(string email) {
            return UserModel.GetUserByEmail(email);
        }

        public static UserEntity GetUserByCredentials(string email, string password) {
            UserEntity foundUser = GetUserByEmail(email);

            if (foundUser == null)
                throw new Exception("User not found!");

            if (foundUser.Password != password)
                throw new Exception("Invalid Credentials!");

            return foundUser;
        }

        public static void CreateUser(string email, string password, string name) {
            UserEntity existingUser = GetUserByEmail(email);

            if (existingUser != null)
                throw new Exception("User already exists with the same email!");

            UserModel.CreateUser(email, password, name);
        }

        public static void DeleteUser(int id) {
            UserModel.DeleteUser(id);
        }

        public static UserEntity UpdateUser(int id, string email, string password, string name, int role) {
            UserEntity updatedUser = GetUser(id);
            UserEntity existingUser = GetUserByEmail(email);

            if (existingUser != null && updatedUser.ToString() != existingUser.ToString())
                throw new Exception("User already exists with the same email!");

            UserModel.UpdateUser(id, email, password, name, role);

            return UserModel.GetUser(id);
        }

    }
}