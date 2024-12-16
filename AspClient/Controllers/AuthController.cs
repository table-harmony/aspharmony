using AspClient.Services;
using AspClient.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspClient.Controllers {

    public class AuthController {
        private readonly AuthService _authService = new AuthService();

        public async Task<bool> LoginAsync(string email, string password) {
            try {
                var user = await _authService.LoginAsync(email, password);

                if (user == null)
                    return false;

                SessionManager.SetCurrentUser(user);
                return true;
            } catch {
                return false;
            }
        }

        public async Task<bool> RegisterAsync(string email, string password) {
            try {
                var user = await _authService.RegisterAsync(email, password);

                if (user == null)
                    return false;

                SessionManager.SetCurrentUser(user);
                return true;
            } catch {
                return false;
            }
        }

        public async Task LogoutAsync() {
            await _authService.LogoutAsync();
            SessionManager.ClearCurrentUser();
        }
    }
}


