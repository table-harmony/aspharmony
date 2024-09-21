using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Models;
using System.Threading.Tasks;
using DataAccessLayer.Entities;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        
        public AccountController(UserManager<User> userManager, 
                                    SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            
            ModelState.AddModelError(string.Empty, "Invalid register attempt.");

            return View(model);
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {

            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
             
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}