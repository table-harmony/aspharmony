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
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
                    INotificationService notificationService, IEventPublisher eventPublisher) : Controller {
        
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    await userManager.AddToRoleAsync(user, "Member");
                    await eventPublisher.PublishUserRegistered(user);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (!ModelState.IsValid) 
                return View(model);

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return View(model);
            }
            
            var user = await userManager.FindByEmailAsync(model.Email);
            await eventPublisher.PublishUserLoggedIn(user);
            
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index() {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public Task<IActionResult> Update() {
            return Task.FromResult<IActionResult>(View(new UpdatePasswordViewModel()));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model) {
            if (!ModelState.IsValid)
                return View("Update", model);

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded) {
                await eventPublisher.PublishUserUpdated(user);
                await signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Update", model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete() {
            var user = await userManager.GetUserAsync(User);
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
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded) {
                await eventPublisher.PublishUserDeleted(user);
                await signInManager.SignOutAsync();
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
            var notifications = await notificationService.GetByUserAsync(userId);

            return View(notifications);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int id) {
            await notificationService.DeleteAsync(id);
            return RedirectToAction(nameof(Notifications));
        }

    }
}