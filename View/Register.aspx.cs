using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspharmony.Controller;

namespace aspharmony.View {
    public partial class Register : System.Web.UI.Page {
        public string username, password, gmail, msg;
        public int gender;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["username"] != null)
                Response.Redirect("Home.aspx");

            if (Request.Form["submit"] != null) {

                CollectFormData();

                if (ControllerUser.CreateUser(username, password, gmail, gender)) {
                    DataTable userData = ControllerUser.GetUserData(username);
                    Session["username"] = username;
                    Session["password"] = password;
                    Session["accessKey"] = int.Parse(userData.Rows[0]["accesskeyField"].ToString());
                    Response.Redirect("Home.aspx");
                }
                else
                    msg = "<p style='color: red'> user already exists <p>";

            }
        }

        private void CollectFormData() {
            username = Request.Form["username"];
            password = Request.Form["password"];
            gender = int.Parse(Request.Form["gender"]);
            gmail = Request.Form["gmail"];
        }
    }
}