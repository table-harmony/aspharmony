using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PresentationLayer.Models;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Events;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    public class AccountController(UserManager<User> userManager, SignInManager<User> signInManager, 
                    INotificationService notificationService, IEventPublisher eventPublisher, 
                    ISenderService senderService, IUserSenderService userSenderService) : Controller {
        
        [HttpGet]
        public IActionResult Register() {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User { 
                UserName = model.Email,
                Email = model.Email,
            };
                
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await userManager.AddToRoleAsync(user, "Member");
            await signInManager.SignInAsync(user, isPersistent: false);

            await eventPublisher.PublishUserRegistered(user);
                
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login() {
            return View(new LoginViewModel() { });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (!ModelState.IsValid) 
                return View(model);

            User? user = await userManager.FindByEmailAsync(model.Email);

            if (user == null || user.UserName == null) {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            
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
            var viewModel = await BuildAccountViewModel();
            
            if (TempData["StatusMessage"] is string statusMessage)
                ViewBag.StatusMessage = statusMessage;
                
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword([Bind(Prefix = "UpdatePassword")] UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await BuildAccountViewModel();
                viewModel.UpdatePassword = model;
                return View("Index", viewModel);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded) {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                var viewModel = await BuildAccountViewModel();
                viewModel.UpdatePassword = model;
                return View("Index", viewModel);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            await eventPublisher.PublishUserUpdated(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            DeleteViewModel model = new() {
                UserName = user.UserName ?? ""
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

            await eventPublisher.PublishUserDeleted(user);

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded) {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Notifications(int pageIndex = 1, int pageSize = 10, bool unreadOnly = false) {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var notifications = unreadOnly 
                ? await notificationService.GetUnreadByUserAsync(user.Id)
                : await notificationService.GetByUserAsync(user.Id);
                
            var senders = await senderService.GetAllAsync();
            var userSenders = await userSenderService.GetByUserIdAsync(user.Id);

            var model = new NotificationsViewModel {
                Notifications = PaginatedList<Notification>.Create(
                    notifications.OrderBy(n => n.CreatedAt),
                    pageIndex,
                    pageSize
                ),
                SenderOptions = senders.Select(s => new SenderOptionViewModel {
                    Id = s.Id,
                    Name = s.Name,
                    IsEnabled = userSenders.Any(us => us.SenderId == s.Id)
                }).ToList(),
                PageSize = pageSize,
                UnreadOnly = unreadOnly
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int id) {
            await notificationService.DeleteAsync(id);
            return RedirectToAction(nameof(Notifications));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSender(int senderId, bool isEnabled) {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound();
            }

            if (isEnabled) {
                await userSenderService.CreateAsync(new UserSender { 
                    UserId = user.Id, 
                    SenderId = senderId 
                });
            }
            else {
                await userSenderService.DeleteAsync(user.Id, senderId);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind(Prefix = "UpdateProfile")] UpdateProfileViewModel model) {
            if (!ModelState.IsValid)
            {
                var viewModel = await BuildAccountViewModel();
                viewModel.UpdateProfile = model;
                return View("Index", viewModel);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            // Check for email before updating
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != user.Id) {
                ModelState.AddModelError("UpdateProfile.Email", "Email is already taken");
                var viewModel = await BuildAccountViewModel();
                viewModel.UpdateProfile = model;
                return View("Index", viewModel);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                var viewModel = await BuildAccountViewModel();
                viewModel.UpdateProfile = model;
                return View("Index", viewModel);
            }

            await eventPublisher.PublishUserUpdated(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id) {
            await notificationService.MarkAsReadAsync(id);
            return RedirectToAction(nameof(Notifications));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            await notificationService.MarkAllAsReadAsync(user.Id);
            return RedirectToAction(nameof(Notifications));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll() {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            await notificationService.DeleteAsync(user.Id);
            return RedirectToAction(nameof(Notifications));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsUnread(int id) {
            await notificationService.MarkAsUnReadAsync(id);
            return RedirectToAction(nameof(Notifications));
        }

        private async Task<AccountViewModel> BuildAccountViewModel() {
            var user = await userManager.GetUserAsync(User);
            return user == null
                ? throw new ApplicationException("Unable to load user.")
                : new AccountViewModel {
                UnreadNotificationsCount = await notificationService.GetUnreadCountAsync(user.Id),
                UpdateProfile = new UpdateProfileViewModel {
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    PhoneNumber = user.PhoneNumber
                },
                UpdatePassword = new UpdatePasswordViewModel()
            };
        }

    }
}
