using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;
using BusinessLogicLayer.Services;

namespace WebApi.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationsController(INotificationService notificationService) : ControllerBase {

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetByUser(string userId) {
        var notifications = await notificationService.GetByUserAsync(userId);
        return Ok(notifications);
    }

    [HttpGet("user/{userId}/unread")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadByUser(string userId) {
        var notifications = await notificationService.GetUnreadByUserAsync(userId);
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request) {
        await notificationService.CreateAsync(request.UserId, request.Message);
        return Ok();
    }

    [HttpPost("{id}/mark-read")]
    public async Task<IActionResult> MarkAsRead(int id) {
        await notificationService.MarkAsReadAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/mark-unread")]
    public async Task<IActionResult> MarkAsUnread(int id) {
        await notificationService.MarkAsUnReadAsync(id);
        return NoContent();
    }

    [HttpPost("user/{userId}/mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead(string userId) {
        await notificationService.MarkAllAsReadAsync(userId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await notificationService.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteAllForUser(string userId) {
        await notificationService.DeleteAsync(userId);
        return NoContent();
    }
}

public class CreateNotificationRequest {
    public string UserId { get; set; } = "";
    public string Message { get; set; } = "";
}