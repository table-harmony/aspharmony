using AspClient.Controllers;
using AspClient.Utils;
using System;
using System.Linq;

namespace AspClient {
    public partial class DeleteLibrary : System.Web.UI.Page {
        private readonly LibraryController _libraryController = new LibraryController();

        protected async void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack) {
                string idParam = Request.QueryString["id"];
                if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out int libraryId)) {
                    Response.Redirect("Libraries.aspx");
                    return;
                }

                var library = await _libraryController.GetLibraryAsync(libraryId);
                if (library == null) {
                    Response.Redirect("Libraries.aspx");
                    return;
                }

                var membership = library.Members.FirstOrDefault(m => m.User.Id == SessionManager.CurrentUser.Id);
                if (membership?.Role != "Manager") {
                    Response.Redirect("Libraries.aspx");
                    return;
                }

                LibraryIdHidden.Value = libraryId.ToString();
                LibraryNameLiteral.Text = library.Name;
            }
        }

        protected async void DeleteButton_Click(object sender, EventArgs e) {
            try {
                if (!int.TryParse(LibraryIdHidden.Value, out int libraryId)) {
                    ClientScript.RegisterStartupScript(GetType(), "redirect",
                        $"window.location = 'Libraries.aspx';", true);
                    return;
                }

                await _libraryController.DeleteAsync(libraryId);

                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    $"window.location = 'Libraries.aspx';", true);
            } catch (Exception) {
                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    $"window.location = '500.aspx';", true);
            }
        }
    }
}