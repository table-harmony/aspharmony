using aspharmony.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View {
    public partial class Delete : System.Web.UI.Page {
        public string msg;

        protected void Page_Load(object sender, EventArgs e) {

            if (Session["id"] == null)
                Response.Redirect("Home.aspx");

            if (Request.Form["submit"] != null)
                Action();
        }

        private void Action() {
            try {
                int userId = int.Parse(Session["id"].ToString());

                ControllerUser.DeleteUser(userId);
                Session.Abandon();

                Response.Redirect("Home.aspx");
            } catch (Exception error) {
                msg = error.Message;
            }
        }
    }
}