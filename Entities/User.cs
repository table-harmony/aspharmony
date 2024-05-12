using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace aspharmony.Entities {
    public class UserEntity {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Role { get; set; }

        public UserEntity() { }

        public UserEntity(DataRow row) {
            if (row.Table.Columns.Contains("id"))
                Id = Convert.ToInt32(row["id"]);

            if (row.Table.Columns.Contains("email"))
                Email = row["email"].ToString();

            if (row.Table.Columns.Contains("password"))
                Password = row["password"].ToString();

            if (row.Table.Columns.Contains("name"))
                Name = row["name"].ToString();

            if (row.Table.Columns.Contains("createdAt") && DateTime.TryParse(row["createdAt"].ToString(), out DateTime createdAt))
                CreatedAt = createdAt;

            if (row.Table.Columns.Contains("role"))
                Role = Convert.ToInt32(row["role"]);
        }

        public override string ToString() {
            return $"Id: {Id}, Email: {Email}, Password: {Password}, " +
                $"Name: {Name}, CreatedAt: {CreatedAt}, Role: {Role}";
        }
    }
}
