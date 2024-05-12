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
    public partial class Register : System.Web.UI.Page {
        public string email, password, name;

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
                UserController.CreateUser(email, password, name);

                Response.Redirect("Login.aspx");
            } catch (Exception error) {
                Response.Write(error.Message);
            }
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            name = Request.Form["name"];
        }
    }
}