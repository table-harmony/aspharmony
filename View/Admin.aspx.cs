using aspharmony.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace aspharmony.View {
    public partial class Admin : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {
            Middleware();

            if (!IsPostBack)
                BindGrid();
        }

        protected void Middleware() {
            if (Session["id"] == null)
                Response.Redirect("Home.aspx");

            if (int.Parse(Session["role"].ToString()) == 1)
                Response.Redirect("Home.aspx");
        }

        protected void BindGrid() {

            UsersGrid.DataSource = ControllerUser.GetUsers();
            UsersGrid.DataBind();

            UsersGrid.SelectedIndex = 0;
        }

        protected void UsersGrid_Sorting(object sender, GridViewSortEventArgs e) {

            DataTable dt = ControllerUser.GetUsers();

            DataView dataView = new DataView(dt);
            dataView.Sort = e.SortExpression;

            UsersGrid.DataSource = dataView;
            UsersGrid.DataBind();
        }

        protected void UsersGrid_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            UsersGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void UsersGrid_RowEditing(object sender, GridViewEditEventArgs e) {
            UsersGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void UsersGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) {
            UsersGrid.EditIndex = -1;
            BindGrid();
        }

        protected void UsersGrid_RowUpdating(object sender, GridViewUpdateEventArgs e) {
            GridViewRow row = UsersGrid.Rows[e.RowIndex];
            
            //TODO: get data from row
            //TODO: update row by data

            UsersGrid.EditIndex = -1;
            BindGrid();
        }

        protected void UsersGrid_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            GridViewRow row = UsersGrid.Rows[e.RowIndex];

            //TODO: get data by row
            //TODO: delete row

            UsersGrid.EditIndex = -1;
            BindGrid();
        }
    }
}