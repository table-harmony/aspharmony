using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll() {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(string id) {
        var user = await userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetByEmail(string email) {
        var user = await userService.GetByEmailAsync(email);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserModel model) {
        await userService.CreateAsync(model.Email, model.Password);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] User user) {
        if (id != user.Id) return BadRequest();
        await userService.UpdateAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await userService.DeleteAsync(id);
        return NoContent();
    }
}

public class CreateUserModel {
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
