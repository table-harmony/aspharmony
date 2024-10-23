using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using PresentationLayer.Models;
using BusinessLogicLayer.Events;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(UserManager<User> userManager,
                            IEventPublisher eventPublisher,
                            ILogger<AdminController> logger) : Controller {
        [HttpGet]
        public IActionResult Index() {
            var users = userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id) {
            var user = await userManager.FindByIdAsync(id);
            
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id) {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id) {
            User? existingUser = await userManager.FindByIdAsync(id);

            if (existingUser == null)
                return NotFound();

            await eventPublisher.PublishUserDeleted(existingUser);
            await userManager.DeleteAsync(existingUser);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            User? user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            EditUserViewModel model = new() {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            User existingUser = await userManager.FindByIdAsync(model.Id);
            existingUser.UserName = model.UserName;

            await eventPublisher.PublishUserUpdated(existingUser);
            await userManager.UpdateAsync(existingUser);

            return RedirectToAction("Index");
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            try {
                User existingUser = await userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                    throw new Exception("User already exists");

                User user = new() {
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber
                };
                
                var res = await userManager.CreateAsync(user, model.Password);
                
                if (!res.Succeeded)
                    throw new Exception("An error occurred while updating");

                await userManager.AddToRoleAsync(user, "Member");

                return RedirectToAction("Index");
            } catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
