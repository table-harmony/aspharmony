using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { 
            Middleware();

            if (Request.Form["submit"] != null) {
                Session.Abandon();
                Response.Redirect("Home.aspx");
            }
        }

        protected void Middleware() {
            if (Session["id"] == null)
                Response.Redirect("Home.aspx");
        }

    }
}