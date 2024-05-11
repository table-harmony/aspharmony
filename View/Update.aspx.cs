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
        public string email, password, name, msg;
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
                ControllerUser.UpdateUser(id, email, password, name, role);
                AppendSession();
            } catch (Exception error) {
                msg = error.Message;
            }
        }

        protected void AppendSession() {
            Session["email"] = email;
            Session["name"] = name;
            Session["role"] = role;
        }

        protected void SetFormData() {
            id = int.Parse(Session["id"].ToString());
            role = int.Parse(Session["role"].ToString());

            DataTable user = ControllerUser.GetUser(id);

            email = user.Rows[0]["email"].ToString();
            password = user.Rows[0]["password"].ToString();
            name = user.Rows[0]["name"].ToString();
        }

        protected void CollectFormData() {
            email = Request.Form["email"];
            password = Request.Form["password"];
            name = Request.Form["name"];
        }
    }
}