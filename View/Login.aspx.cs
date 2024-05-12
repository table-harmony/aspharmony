using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspharmony.Controller;
using aspharmony.Entities;

namespace aspharmony.View
{
    public partial class Login : System.Web.UI.Page 
    {
        public string email, password;

        protected void Page_Load(object sender, EventArgs e) {
            Middleware();

            if (Request.Form["submit"] != null) {
                CollectFormData();
                Action();
            }
        }

        protected void Middleware() {
            if (Session["id"] != null)
                Response.Redirect("Home.aspx");
        }

        protected void Action() {
            try {
                UserEntity user = UserController.GetUserByCredentials(email, password);
                AppendSession(user);

                Response.Redirect("Home.aspx");
            } catch (Exception error) {
                Response.Write(error.Message);
            }
        }

        protected void AppendSession(UserEntity user) {
            Session["id"]    = user.Id;
            Session["email"] = user.Email;
            Session["name"]  = user.Name;
            Session["role"]  = user.Role;
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
        }

    }
}