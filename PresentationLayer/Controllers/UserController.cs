using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Utils.Exceptions;
using DataAccessLayer.Entities;

namespace PresentationLayer.Controllers
{
    public class UserController : Controller {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UserController(IUserService userService, UserManager<User> userManager) {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) {

            if (!ModelState.IsValid)
                return View(model);

            try {
                await _userService.CreateAsync(model.Email, model.Password);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)  {
                string error = "Something went wrong";

                if (ex is PublicException)
                    error = ex.Message;

                ModelState.AddModelError("CustomError", error);
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id) {
            try {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                var viewModel = new EditUserViewModel {
                    Id = user.Id,
                    Email = user.Email
                };

                return View(viewModel);
            }
            catch (NotFoundException) {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model) {
            if (id != model.Id) {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(model);

            try {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                user.Email = model.Email;
                user.UserName = model.Email;

                var result = await _userManager.UpdateAsync(user);

                if (!string.IsNullOrEmpty(model.NewPassword)) {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                }

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (Exception ex) {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id) {
            try {
                var user = await _userService.GetByIdAsync(id);
                return View(user);
            }
            catch (NotFoundException) {
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(string id) {
            try {
                var user = await _userService.GetByIdAsync(id);
                return View(user);
            }
            catch (NotFoundException) {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id) {
            try {
                await _userService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException) {
                return NotFound();
            }
        }
    }
}
