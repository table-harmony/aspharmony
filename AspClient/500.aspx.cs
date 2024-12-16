using AspClient.Utils;
using System;
using System.Web;
using System.Web.UI;

namespace AspClient {
    public partial class _500 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var exception = HttpContext.Current.Items["Exception"] as Exception;

            if (exception == null) {
                ErrorMessage.Text = "An unexpected error occurred. Please try again later.";
                return;
            }

            if (exception is PublicException) {
                ErrorMessage.Text = exception.Message;
            } else {
                ErrorMessage.Text = "An unexpected system error occurred. Please try again later.";
            }
        }
    }
}