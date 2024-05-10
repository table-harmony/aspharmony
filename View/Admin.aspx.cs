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

            if (Session["id"] == null)
                Response.Redirect("Home.aspx");

            if (int.Parse(Session["role"].ToString()) == 1)
                Response.Redirect("Home.aspx");

            if (!IsPostBack)
                BindGrid();
        }


        private void BindGrid() {

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
            GridViewRow updatedRow = UsersGrid.Rows[e.RowIndex];

            int id = int.Parse(updatedRow.Cells[0].Text),
                gender = int.Parse(((TextBox)updatedRow.Cells[3].Controls[0]).Text),
                role = int.Parse(((TextBox)updatedRow.Cells[4].Controls[0]).Text);

            string email = ((TextBox)updatedRow.Cells[1].Controls[0]).Text,
                   password = ((TextBox)updatedRow.Cells[1].Controls[0]).Text;

            ControllerUser.UpdateUser(id, email, password, gender, role);

            UsersGrid.EditIndex = -1;
            BindGrid();

        }

        protected void UsersGrid_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            GridViewRow deletedRow = UsersGrid.Rows[e.RowIndex];

            int id = int.Parse(deletedRow.Cells[0].Text);
            ControllerUser.DeleteUser(id);

            UsersGrid.EditIndex = -1;
            BindGrid();
        }
    }
}