using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Models;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using System.Security.Claims;
using BusinessLogicLayer.Events;

namespace PresentationLayer.Controllers
{
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly INotificationService _notificationService;
        private readonly IEventPublisher _eventPublisher;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, INotificationService notificationService, IEventPublisher eventPublisher) {
            _userManager = userManager;
            _signInManager = signInManager;
            _notificationService = notificationService;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Member");
                    await _eventPublisher.PublishUserRegistered(user);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (!ModelState.IsValid) 
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return View(model);
            }
            
            var user = await _userManager.FindByEmailAsync(model.Email);
            await _eventPublisher.PublishUserLoggedIn(user);
            
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update() {
            return View(new UpdatePasswordViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model) {
            if (!ModelState.IsValid)
                return View("Update", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) {
                await _eventPublisher.PublishUserUpdated(user);
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Update", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var model = new DeleteViewModel {
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) {
                await _eventPublisher.PublishUserDeleted(user);
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            var model = new DeleteViewModel {
                UserName = user.UserName
            };

            return View("Profile", model);
        }

        [HttpGet]
        public async Task<IActionResult> Notifications() {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationService.GetByUserAsync(userId);

            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int id) {
            await _notificationService.DeleteAsync(id);
            return RedirectToAction(nameof(Notifications));
        }

    }
}