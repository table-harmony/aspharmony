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
        public string email, password, msg;
        public int gender;

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
                ControllerUser.CreateUser(email, password, gender);

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
            Session["role"] = int.Parse(user.Rows[0]["role"].ToString());
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            gender = int.Parse(Request.Form["gender"]);
        }
    }
}