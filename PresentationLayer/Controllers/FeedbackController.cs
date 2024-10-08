using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PresentationLayer.Models;
using System.Security.Claims;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace PresentationLayer.Controllers
{
    public class FeedbackController(IFeedbackService feedbackService) : Controller {
        [HttpGet]
        [Authorize]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFeedbackViewModel model) {
            if (ModelState.IsValid) {
                Feedback feedback = new() {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    Title = model.Title,
                    Description = model.Description,
                    Label = model.Label,
                };

                await feedbackService.CreateAsync(feedback);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index() {
            var feedbacks = await feedbackService.GetAllAsync();
            return View(feedbacks.Tables[0]);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            await feedbackService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}