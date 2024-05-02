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

        public string username, password, msg;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["username"] != null)
                Response.Redirect("Home.aspx");

            if (Request.Form["submit"] != null) {

                CollectFormData();

                if (ControllerUser.IsUserExist(username, password)) {
                    DataTable userData = ControllerUser.GetUserData(username);
                    Session["username"] = username;
                    Session["password"] = password;
                    Session["accessKey"] = int.Parse(userData.Rows[0]["accesskeyField"].ToString());
                    Response.Redirect("Home.aspx");
                }
                else
                    msg = "<p style='color: red'> user doesn't exist <p>";

            }
            
        }

        private void CollectFormData() {
            username = Request.Form["username"];
            password = Request.Form["password"];
        }

    }
}