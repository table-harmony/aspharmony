using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

using Book = BusinessLogicLayer.Services.Book;

namespace WebApi.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll([FromQuery] ServerType? serverType) {
        var books = serverType.HasValue ? 
            await bookService.GetAllAsync(serverType.Value) : 
            await bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id) {
        var book = await bookService.GetBookAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create([FromBody] Book book) {
        await bookService.CreateAsync(book);
        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Book book) {
        if (id != book.Id) return BadRequest();
        await bookService.UpdateAsync(book);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await bookService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/audio")]
    public async Task<ActionResult<AudioBook>> GetAudioBook(int id) {
        var audioBook = await bookService.GetAudioBookAsync(id);
        if (audioBook == null) return NotFound();
        return Ok(audioBook);
    }

    [HttpPost("{id}/audio")]
    public async Task<IActionResult> CreateAudioBook([FromBody] AudioBook audioBook) {
        await bookService.CreateAudioBookAsync(audioBook);
        return CreatedAtAction(nameof(GetAudioBook), new { id = audioBook.Id }, audioBook);
    }

    [HttpDelete("{id}/audio")]
    public async Task<IActionResult> DeleteAudioBook(int id) {
        await bookService.DeleteAudioBookAsync(id);
        return NoContent();
    }
} 