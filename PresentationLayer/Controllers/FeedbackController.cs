using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PresentationLayer.Models;
using System.Security.Claims;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using System.Data;

namespace PresentationLayer.Controllers {
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
        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageIndex = 1) {
            var feedbacks = await feedbackService.GetAllAsync();
            
            if (!string.IsNullOrEmpty(searchString)) {
                feedbacks = feedbacks.Where(f => 
                    f.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) || 
                    f.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    f.Label.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    f.User.UserName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                );
            }

            var paginatedFeedbacks = PaginatedList<Feedback>.Create(
                feedbacks.OrderByDescending(f => f.Id),
                pageIndex,
                pageSize
            );

            var model = new FeedbackIndexViewModel {
                Feedbacks = paginatedFeedbacks,
                SearchString = searchString,
                PageSize = pageSize
            };

            return View(model);
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