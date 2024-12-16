using AspClient.Controllers;
using AspClient.Utils;
using System;

namespace AspClient {
    public partial class CreateLibrary : System.Web.UI.Page {
        private readonly LibraryController _libraryController = new LibraryController();

        protected void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Login.aspx");
            }
        }

        protected async void CreateButton_Click(object sender, EventArgs e) {
            try {
                var library = await _libraryController.CreateAsync(
                    NameTextBox.Text,
                    AllowCopiesCheckBox.Checked
                );

                await _libraryController.JoinAsync(
                   library.Id,
                   SessionManager.CurrentUser.Id,
                   "Manager"
               );

                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    "window.location = 'Libraries.aspx';", true);
            } catch (Exception) {
                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    "window.location = '500.aspx';", true);
            }
        }
    }
}