using AspClient.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspClient {
    public partial class Libraries : System.Web.UI.Page {
        private readonly LibraryController libraryController = new LibraryController();

        protected async void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                var libraries = await libraryController.GetAllAsync();
                LibrariesGridView.DataSource = libraries;
                LibrariesGridView.DataBind();
            }
        }
    }
}