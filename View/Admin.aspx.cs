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
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) {

            if (Session["accesskey"] == null)
                Response.Redirect("Home.aspx");
            if (int.Parse(Session["accesskey"].ToString()) == 1)
                Response.Redirect("Home.aspx");


            if (!IsPostBack)
                BindGrid();
        }


        private void BindGrid() {

            UsersGrid.DataSource = ControllerUser.GetAllUsers();
            UsersGrid.DataBind();

            UsersGrid.SelectedIndex = 0;
        }

        protected void UsersGrid_Sorting(object sender, GridViewSortEventArgs e) {

            DataTable dt = ControllerUser.GetAllUsers();

            DataView dataView = new DataView(dt);
            dataView.Sort = e.SortExpression;

            UsersGrid.DataSource = dataView;
            UsersGrid.DataBind();
        }

        protected void UsersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            UsersGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}