using AspClient.Controllers;
using AspClient.Services;
using AspClient.Utils;
using System;

namespace AspClient {
    public partial class BookDetails : System.Web.UI.Page {
        private readonly BookController _bookController = new BookController();

        protected async void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Home.aspx");
            }

            if (!IsPostBack) {
                string idParam = Request.QueryString["id"];
                if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out int bookId)) {
                    Response.Redirect("~/Books.aspx");
                    return;
                }

                var book = await _bookController.GetBookAsync(bookId);
                if (book == null) {
                    Response.Redirect("~/Books.aspx");
                    return;
                }

                BookTitle.Text = book.Metadata.Title;
                BookImage.ImageUrl = book.Metadata.ImageUrl;
                AuthorName.Text = book.Author?.Username ?? "Unknown";
                BookDescription.Text = book.Metadata.Description;
                ServerType.Text = book.Server.ToString();

                if (SessionManager.CurrentUser.Id == book.Author.Id) {
                    AuthorPanel.Visible = true;
                    EditLink.NavigateUrl = $"~/EditBook.aspx?id={bookId}";
                    DeleteLink.NavigateUrl = $"~/DeleteBook.aspx?id={bookId}";
                }
            }
        }
    }
}