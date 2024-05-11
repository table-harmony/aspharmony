using aspharmony.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace aspharmony.View {
    public partial class Admin : System.Web.UI.Page {
        public string msg;

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

            try {
                GridViewRow row = UsersGrid.Rows[e.RowIndex];
                
                int    id = int.Parse(row.Cells[1].Text);
                string email = ((TextBox)row.Cells[2].Controls[0]).Text;
                string password = ((TextBox)row.Cells[3].Controls[0]).Text;
                string name = ((TextBox)row.Cells[4].Controls[0]).Text;
                int    role = int.Parse(((TextBox)row.Cells[6].Controls[0]).Text);

                ControllerUser.UpdateUser(id, email, password, name, role);

                msg = "User updated successfully!";
            } catch (Exception error) {
                msg = error.Message;
            }

            UsersGrid.EditIndex = -1;
            BindGrid();
        }

        protected void UsersGrid_RowDeleting(object sender, GridViewDeleteEventArgs e) {
            try {
                GridViewRow row = UsersGrid.Rows[e.RowIndex];

                int id = int.Parse(row.Cells[1].Text);
                ControllerUser.DeleteUser(id);

                msg = "User deleted successfully!";
            } catch (Exception error) {
                msg = error.Message;
            }

            UsersGrid.EditIndex = -1;
            BindGrid();
        }
    }
}