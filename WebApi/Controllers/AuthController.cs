using BusinessLogicLayer.Events;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    INotificationService notificationService,
    IEventPublisher eventPublisher,
    ISenderService senderService,
    IUserSenderService userSenderService) : ControllerBase {

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
        User user = new() {
            UserName = request.Email,
            Email = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await userManager.AddToRoleAsync(user, "Member");
        await signInManager.SignInAsync(user, isPersistent: false);
        await eventPublisher.PublishUserRegistered(user);

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null || user.UserName == null)
            return BadRequest("Invalid login attempt");

        var result = await signInManager.PasswordSignInAsync(
            user.UserName,
            request.Password,
            false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
            return BadRequest("Invalid login attempt");

        await eventPublisher.PublishUserLoggedIn(user);
        return Ok(user);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckAuth() {
        if (User.Identity?.IsAuthenticated != true)
            return Ok(false);

        var user = await userManager.GetUserAsync(User);
        return Ok(user != null);
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] bool unreadOnly = false) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var notifications = unreadOnly
            ? await notificationService.GetUnreadByUserAsync(userId)
            : await notificationService.GetByUserAsync(userId);

        return Ok(notifications);
    }

    [HttpPost("notifications/{id}/read")]
    public async Task<IActionResult> MarkNotificationAsRead(int id) {
        await notificationService.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpPost("notifications/read-all")]
    public async Task<IActionResult> MarkAllNotificationsAsRead() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        await notificationService.MarkAllAsReadAsync(userId);
        return Ok();
    }
}

public class RegisterRequest {
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginRequest {
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}