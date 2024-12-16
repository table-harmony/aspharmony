using AspClient.Controllers;
using AspClient.Utils;
using System;
using System.Web.UI;

namespace AspClient {
    public partial class Register : System.Web.UI.Page {
        private readonly AuthController _authController = new AuthController();

        protected void Page_Load(object sender, EventArgs e) {
            if (SessionManager.CurrentUser != null) {
                Response.Redirect("Home.aspx");
            }
        }

        protected async void RegisterButton_Click(object sender, EventArgs e) {
            try {
                ErrorPanel.Visible = false;

                if (!Page.IsValid) {
                    ErrorMessage.Text = "Page is not valid. Please try again.";
                    ErrorPanel.Visible = true;
                    return;
                }

                bool success = await _authController.RegisterAsync(
                    EmailTextBox.Text, 
                    PasswordTextBox.Text
                );

                if (!success) {
                    ErrorMessage.Text = "Registration failed. Please try again.";
                    ErrorPanel.Visible = true;
                    return;
                }

                ClientScript.RegisterStartupScript(GetType(), "redirect",
                    "window.location = 'Home.aspx';", true);
            } catch {
                ErrorMessage.Text = "An unexpected error occurred. Please try again.";
                ErrorPanel.Visible = true;
            }
        }
    }
}