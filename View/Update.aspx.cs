using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspharmony.Controller;

namespace aspharmony.View
{
    public partial class Update : System.Web.UI.Page {
        public string gmail, password;
        public int gender;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["username"] == null)
                Response.Redirect("Home.aspx");

            DataTable user = ControllerUser.GetUserByUsername(Session["username"].ToString());
            SetFormData(user);

            if (Request.Form["submit"] != null) {
                CollectFormData();
                if (ControllerUser.UpdateUser(Session["username"].ToString(), password, gmail, gender))
                    Response.Redirect("Home.aspx");
            }

        }

        public void SetFormData(DataTable data) {
            gmail = data.Rows[0]["gmailField"].ToString();
            password = data.Rows[0]["passwordField"].ToString();
            gender = Convert.ToInt32(data.Rows[0]["genderField"]);
        }

        private void CollectFormData() {
            gmail = Request.Form["gmail"];
            password = Request.Form["password"];
            gender = int.Parse(Request.Form["gender"].ToString());
        }
    }
}