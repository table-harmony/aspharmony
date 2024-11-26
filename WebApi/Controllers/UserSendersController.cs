using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/user-senders")]
[ApiController]
public class UserSendersController(IUserSenderService userSenderService) : ControllerBase {
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<UserSender>>> GetByUserId(string userId) {
        var userSenders = await userSenderService.GetByUserIdAsync(userId);
        return Ok(userSenders);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserSender userSender) {
        await userSenderService.CreateAsync(userSender);
        return Ok();
    }

    [HttpDelete("user/{userId}/sender/{senderId}")]
    public async Task<IActionResult> Delete(string userId, int senderId) {
        await userSenderService.DeleteAsync(userId, senderId);
        return NoContent();
    }
} 