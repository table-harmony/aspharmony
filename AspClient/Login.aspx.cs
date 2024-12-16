using AspClient.Controllers;
using AspClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspClient {
    public partial class Login : System.Web.UI.Page {
        private readonly AuthController _authController = new AuthController();

        protected void Page_Load(object sender, EventArgs e) {
            if (SessionManager.CurrentUser != null) {
                Response.Redirect("Home.aspx");
            }
        }

        protected async void LoginButton_Click(object sender, EventArgs e) {
            try {
                ErrorPanel.Visible = false;

                if (!Page.IsValid) {
                    ErrorMessage.Text = "Page is not valid. Please try again.";
                    ErrorPanel.Visible = true;
                    return;
                }

                bool success = await _authController.LoginAsync(
                    EmailTextBox.Text,
                    PasswordTextBox.Text
                );

                if (!success) {
                    ErrorMessage.Text = "Login failed. Please try again.";
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