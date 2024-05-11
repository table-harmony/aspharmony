using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace aspharmony.Entities {
    public class User {
        private int id;
        private string email;
        private string password;
        private string name;
        private DateTime createdAt;
        private int role;

        public User(DataTable dt) { 
        }

        public int GetId() { return id; }
        public void SetId(int id) { this.id = id; }

        public string GetEmail() { return email; } 
        public void SetEmail(string email) { this.email = email;}

        public string GetPassword() { return password; }
        public void SetPassword(string password) {  this.password = password; }
        
        public string GetName() { return name; }
        public void SetName(string name) {  this.name = name; }

        public DateTime GetCreatedAt() { return createdAt; }
        
        public int GetRole() { return role; }
        public void SetRole(int role) { this.role = role; }
    }
}
