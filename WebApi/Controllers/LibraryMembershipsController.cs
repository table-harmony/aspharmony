using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/library-memberships")]
[ApiController]
public class LibraryMembershipsController(ILibraryMembershipService membershipService) : ControllerBase {
    [HttpGet("{id}")]
    public async Task<ActionResult<LibraryMembership>> Get(int id) {
        var membership = await membershipService.GetMembershipAsync(id);
        if (membership == null) return NotFound();
        return Ok(membership);
    }

    [HttpGet("library/{libraryId}/user/{userId}")]
    public async Task<ActionResult<LibraryMembership>> GetByLibraryAndUser(int libraryId, string userId) {
        var membership = await membershipService.GetMembershipAsync(libraryId, userId);
        if (membership == null) return NotFound();
        return Ok(membership);
    }

    [HttpGet("library/{libraryId}")]
    public ActionResult<IEnumerable<LibraryMembership>> GetLibraryMembers(int libraryId) {
        var members = membershipService.GetLibraryMembers(libraryId);
        return Ok(members);
    }

    [HttpPost]
    public async Task<ActionResult<LibraryMembership>> Create([FromBody] LibraryMembership membership) {
        await membershipService.CreateAsync(membership);
        return CreatedAtAction(nameof(Get), new { id = membership.Id }, membership);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LibraryMembership membership) {
        if (id != membership.Id) return BadRequest();
        await membershipService.UpdateAsync(membership);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await membershipService.DeleteAsync(id);
        return NoContent();
    }

    [HttpDelete("library/{libraryId}/user/{userId}")]
    public async Task<IActionResult> DeleteByLibraryAndUser(int libraryId, string userId) {
        await membershipService.DeleteAsync(libraryId, userId);
        return NoContent();
    }
} 