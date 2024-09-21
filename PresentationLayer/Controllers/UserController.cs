using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using Utils.Exceptions;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace PresentationLayer.Controllers
{
    public class UserController : Controller {
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserService userService, UserManager<IdentityUser> userManager) {
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

                 ModelState.AddModelError(string.Empty, error);
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id) {
            try {
                var user = await _userService.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(int id, EditUserViewModel model) {
            if (id != model.Id) {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(model);

            try {
                var user = new User { Id = model.Id, Email = model.Email, Password = model.NewPassword };
                await _userService.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                string error = "Something went wrong";

                if (ex is PublicException)
                    error = ex.Message;

                await Response.WriteAsync(error);
                ModelState.AddModelError(string.Empty, error);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id) {
            try {
                var user = await _userService.GetByIdAsync(id);
                return View(user);
            }
            catch (NotFoundException) {
                return NotFound();
            }
        }

        public async Task<IActionResult> Delete(int id) {
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
        public async Task<IActionResult> DeleteConfirmed(int id) {
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
