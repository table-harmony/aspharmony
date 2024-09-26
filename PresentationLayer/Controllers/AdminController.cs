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
                User existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                    throw new Exception("User already exists");

                User user = new() {
                    Email = model.Email,
                    UserName = model.Email
                };
                
                var res = await _userManager.CreateAsync(user, model.Password);
                
                if (!res.Succeeded)
                    throw new Exception("An error occurred while updating");

                await _userManager.AddToRoleAsync(user, "Member");

                return RedirectToAction("Index");
            } catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
