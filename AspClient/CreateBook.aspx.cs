using AspClient.Controllers;
using AspClient.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspClient {
    public partial class CreateBook : System.Web.UI.Page {
        private readonly BookController _bookController = new BookController();
        protected IFileUploader fileUploader = Global.ServiceProvider.GetService<IFileUploader>();

        protected void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Login.aspx");
            }
        }

        protected async void CreateButton_Click(object sender, EventArgs e) {
            try {
                var file = new File {
                    Stream = ImageControl.FileContent,
                    ContentType = ImageControl.PostedFile.ContentType
                };

                string imageUrl = await fileUploader.UploadFileAsync(file);

                await _bookController.CreateAsync(
                    TitleTextBox.Text,
                    DescriptionTextBox.Text,
                    imageUrl ?? "https://via.placeholder.com/400x600?text=No+Cover",
                    int.Parse(ServerDropDown.SelectedValue),
                    SessionManager.CurrentUser.Id
                );

                ClientScript.RegisterStartupScript(GetType(), "redirect", 
                    $"window.location = 'Books.aspx';", true);
            } catch (Exception) {
                ClientScript.RegisterStartupScript(GetType(), "redirect", 
                    "window.location = '500.aspx';", true);
            }
        }
    }
}