using AspClient.Controllers;
using AspClient.Utils;
using System;

namespace AspClient {
    public partial class DeleteBook : System.Web.UI.Page {
        private readonly BookController _bookController = new BookController();

        protected async void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack) {
                string idParam = Request.QueryString["id"];
                if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out int bookId)) {
                    Response.Redirect("Books.aspx");
                    return;
                }

                var book = await _bookController.GetBookAsync(bookId);
                if (book == null || book.Author?.Id != SessionManager.CurrentUser.Id) {
                    Response.Redirect("Books.aspx");
                    return;
                }

                BookIdHidden.Value = bookId.ToString();
                BookTitleLiteral.Text = book.Metadata.Title;
            }
        }

        protected async void DeleteButton_Click(object sender, EventArgs e) {
            try {
                if (!int.TryParse(BookIdHidden.Value, out int bookId)) {
                    ClientScript.RegisterStartupScript(GetType(), "redirect", 
                        "window.location = 'Books.aspx';", true);
                    return;
                }

                await _bookController.DeleteAsync(bookId);
                ClientScript.RegisterStartupScript(GetType(), "redirect", 
                    "window.location = 'Books.aspx';", true);
            } catch (Exception) {
                ClientScript.RegisterStartupScript(GetType(), "redirect", 
                    "window.location = '500.aspx';", true);
            }
        }
    }
}