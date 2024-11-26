using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/library-books")]
[ApiController]
public class LibraryBooksController(ILibraryBookService libraryBookService) : ControllerBase {
    [HttpGet("{id}")]
    public async Task<ActionResult<LibraryBook>> Get(int id) {
        var libraryBook = await libraryBookService.GetLibraryBookAsync(id);
        if (libraryBook == null) return NotFound();
        return Ok(libraryBook);
    }

    [HttpGet("library/{libraryId}")]
    public async Task<ActionResult<IEnumerable<LibraryBook>>> GetByLibrary(int libraryId) {
        var books = await libraryBookService.GetLibraryBooksAsync(libraryId);
        return Ok(books);
    }

    [HttpGet("library/{libraryId}/book/{bookId}")]
    public async Task<ActionResult<IEnumerable<LibraryBook>>> GetByLibraryAndBook(int libraryId, int bookId) {
        var books = await libraryBookService.GetLibraryBooksAsync(libraryId, bookId);
        return Ok(books);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLibraryBookRequest request) {
        await libraryBookService.CreateAsync(request.LibraryId, request.BookId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await libraryBookService.DeleteAsync(id);
        return NoContent();
    }
}

public class CreateLibraryBookRequest {
    public int LibraryId { get; set; }
    public int BookId { get; set; }
} 