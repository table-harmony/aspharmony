using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspharmony.Controller;

namespace aspharmony.View
{
    public partial class Login : System.Web.UI.Page
    {

        public string email, password, msg;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["id"] != null)
                Response.Redirect("Home.aspx");

            if (Request.Form["submit"] != null) {
                CollectFormData();
                Action();
            }
            
        }

        private void Action() {
            try {
                DataTable user = ControllerUser.GetUserByCredentials(email, password);

                AppendSession(user);

                Response.Redirect("Home.aspx");
            } catch (Exception error) {
                msg = error.Message;
            }
        }

        private void AppendSession(DataTable user) {
            Session["id"] = int.Parse(user.Rows[0]["id"].ToString());
            Session["email"] = email;
            Session["role"] = int.Parse(user.Rows[0]["role"].ToString());
        }

        private void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
        }

    }
}