using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;
using BusinessLogicLayer.Services;

namespace WebApi.Controllers;

[Route("api/libraries")]
[ApiController]
public class LibraryController(ILibraryService libraryService) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Library>>> GetAll() {
        var libraries = await libraryService.GetAllAsync();
        return Ok(libraries);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Library>> Get(int id) {
        var library = await libraryService.GetLibraryAsync(id);
        if (library == null) return NotFound();
        return Ok(library);
    }

    [HttpPost]
    public async Task<ActionResult<Library>> Create([FromBody] Library library) {
        await libraryService.CreateAsync(library);
        return CreatedAtAction(nameof(Get), new { id = library.Id }, library);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Library library) {
        if (id != library.Id) return BadRequest();
        await libraryService.UpdateAsync(library);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await libraryService.DeleteAsync(id);
        return NoContent();
    }
}