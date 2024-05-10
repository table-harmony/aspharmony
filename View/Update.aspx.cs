using aspharmony.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View {
    public partial class Update : System.Web.UI.Page {
        public string email, password, msg;
        public int id, gender, role;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["id"] == null)
                Response.Redirect("Home.aspx");

            SetFormData();

            if (Request.Form["submit"] != null) {
                CollectFormData();
                Action();
            }

        }

        private void Action() {
            try {
                ControllerUser.UpdateUser(id, email, password, gender, role);
                AppendSession();
            } catch (Exception error) {
                msg = error.Message;
            }
        }

        private void AppendSession() {
            Session["email"] = email;
        }

        private void SetFormData() {
            id = int.Parse(Session["id"].ToString());
            role = int.Parse(Session["role"].ToString());

            DataTable user = ControllerUser.GetUser(id);

            email = user.Rows[0]["email"].ToString();
            password = user.Rows[0]["password"].ToString();
            gender = Convert.ToInt32(user.Rows[0]["gender"]);
        }

        private void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            gender = int.Parse(Request.Form["gender"].ToString());
        }
    }
}