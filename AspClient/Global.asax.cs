using AspClient.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Web;

namespace AspClient {
    public class Global : System.Web.HttpApplication {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected void Application_Start(object sender, EventArgs e) {
            System.Web.UI.ValidationSettings.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            var services = new ServiceCollection();
            services.AddScoped<IFileUploader, CloadFileUploader>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected void Session_Start(object sender, EventArgs e) {

        }

        protected void Application_BeginRequest(object sender, EventArgs e) {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        }

        protected void Application_Error(object sender, EventArgs e) {
            
        }

        protected void Session_End(object sender, EventArgs e) {

        }

        protected void Application_End(object sender, EventArgs e) {

        }
    }
}