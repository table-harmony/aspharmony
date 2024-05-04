using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspharmony.Controller;

namespace aspharmony.View
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["username"] == null)
                Response.Redirect("Home.aspx");

            ControllerUser.DeleteUserByUsername(Session["username"].ToString());
            Session.Abandon();
            Response.Redirect("Home.aspx");
        }

    }
}