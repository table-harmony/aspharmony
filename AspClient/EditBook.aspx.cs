using AspClient.Controllers;
using AspClient.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspClient {
    public partial class EditBook : System.Web.UI.Page {
        private readonly BookController _bookController = new BookController();
        protected IFileUploader fileUploader = Global.ServiceProvider.GetService<IFileUploader>();

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
                TitleTextBox.Text = book.Metadata.Title;
                DescriptionTextBox.Text = book.Metadata.Description;
                CurrentImage.ImageUrl = book.Metadata.ImageUrl;
            }
        }

        protected async void UpdateButton_Click(object sender, EventArgs e) {
            try {
                if (!int.TryParse(BookIdHidden.Value, out int bookId)) {
                    ClientScript.RegisterStartupScript(GetType(), "redirect",
                        "window.location = 'Books.aspx';", true);
                    return;
                }

                string imageUrl = CurrentImage.ImageUrl;
                if (ImageControl.HasFile) {
                    var file = new File {
                        Stream = ImageControl.FileContent,
                        ContentType = ImageControl.PostedFile.ContentType
                    };
                    imageUrl = await fileUploader.UploadFileAsync(file);
                }

                await _bookController.UpdateAsync(
                    bookId,
                    SessionManager.CurrentUser.Id,
                    5,
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    imageUrl
                );

                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    $"window.location = 'BookDetails.aspx?id={bookId}';", true);
            } catch (Exception) {
                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    "window.location = '500.aspx';", true);
            }
        }
    }
}