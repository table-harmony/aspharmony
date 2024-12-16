using AspClient.Controllers;
using System;
using System.Web.UI;
using AspClient.Utils;

namespace AspClient {
    public partial class LogoutConfirmation : System.Web.UI.Page {
        private readonly AuthController _authController = new AuthController();

        protected void Page_Load(object sender, EventArgs e) {
            if (!SessionManager.IsUserLoggedIn()) {
                Response.Redirect("Home.aspx");
            }
        }

        protected async void LogoutButton_Click(object sender, EventArgs e) {
            await _authController.LogoutAsync();
            Response.Redirect("Home.aspx");
        }

        protected void CancelButton_Click(object sender, EventArgs e) {
            Response.Redirect(Request.UrlReferrer?.ToString() ?? "Home.aspx");
        }
    }
}