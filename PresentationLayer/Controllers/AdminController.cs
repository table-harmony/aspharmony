using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BusinessLogicLayer.Services;
using PresentationLayer.Models;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<User> userManager,
                                ILogger<AdminController> logger) {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id) {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id) {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id) {
            User existingUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(existingUser);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            var user = await _userManager.FindByIdAsync(id);

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

            User existingUser = await _userManager.FindByIdAsync(model.Id);
            existingUser.UserName = model.UserName;

            await _userManager.UpdateAsync(existingUser);

            return RedirectToAction("Index");
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model) {

            try {
                User user = new() {
                    Email = model.Email,
                    PasswordHash = model.Password
                };
                await _userManager.CreateAsync(user);

                return RedirectToAction("Index");
            } catch (Exception ex) {
                ModelState.AddModelError("", "An error occurred while updating.");
                return View(model);
            }
        }
    }
}
