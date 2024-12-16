using AspClient.Controllers;
using AspClient.Utils;
using System;
using System.Linq;

namespace AspClient {
    public partial class LibraryDetails : System.Web.UI.Page {
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

                LibraryName.Text = library.Name;
                LibraryNameBreadcrumb.Text = library.Name;
                AllowCopies.Text = library.AllowCopies ? "Yes" : "No";

                BooksGridView.DataSource = library.Books;
                BooksGridView.DataBind();
                TotalBooks.Text = library.Books.Count.ToString();

                MembersGridView.DataSource = library.Members;
                MembersGridView.DataBind();

                var membership = library.Members
                    .Where(member => member.User.Id == SessionManager.CurrentUser.Id)
                    .FirstOrDefault();

                if (membership?.Role == "Manager") {
                    ManagerPanel.Visible = true;
                    EditLink.NavigateUrl = $"~/EditLibrary.aspx?id={libraryId}";
                    DeleteLink.NavigateUrl = $"~/DeleteLibrary.aspx?id={libraryId}";
                }
            }
        }
    }
}