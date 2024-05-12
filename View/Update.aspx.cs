using aspharmony.Controller;
using aspharmony.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View {
    public partial class Update : System.Web.UI.Page {
        public string email, password, name;
        public int id, role;

        protected void Page_Load(object sender, EventArgs e) {
            Middleware();

            SetFormData();

            if (Request.Form["submit"] != null) {
                CollectFormData();
                Action();
            }

        }

        protected void Middleware() {
            if (Session["id"] == null)
                Response.Redirect("Home.aspx");
        }

        protected void Action() {
            try {
                UserEntity user = UserController.UpdateUser(id, email, password, name, role);
                AppendSession(user);
            } catch (Exception error) {
                Response.Write(error.Message);
            }
        }

        protected void AppendSession(UserEntity user) {
            Session["email"] = user.Email;
            Session["name"] = user.Name;
            Session["role"] = user.Role;
        }

        protected void SetFormData() {
            id = int.Parse(Session["id"].ToString());
            role = int.Parse(Session["role"].ToString());

            UserEntity user = UserController.GetUser(id);

            email = user.Email;
            password = user.Password;
            name = user.Name;
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            name = Request.Form["name"];
        }
    }
}