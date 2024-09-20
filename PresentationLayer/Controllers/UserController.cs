using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;

namespace PresentationLayer.Controllers {
    public class UserController : Controller {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }


        public async Task<IActionResult> Index() {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

    }
}
