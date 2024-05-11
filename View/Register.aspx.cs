using aspharmony.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View {
    public partial class Register : System.Web.UI.Page {
        public string email, password, name, msg;

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
                ControllerUser.CreateUser(email, password, name);

                DataTable user = ControllerUser.GetUserByEmail(email);
                AppendSession(user);

                Response.Redirect("Home.aspx");
            } catch (Exception error) {
                msg = error.Message;
            }
        }

        protected void AppendSession(DataTable user) {
            Session["id"] = int.Parse(user.Rows[0]["id"].ToString());
            Session["email"] = email;
            Session["name"] = name;
            Session["role"] = int.Parse(user.Rows[0]["role"].ToString());
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            name = Request.Form["name"];
        }
    }
}