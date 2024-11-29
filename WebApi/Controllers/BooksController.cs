using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Book = BusinessLogicLayer.Services.Book;

namespace WebApi.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAll([FromQuery] ServerType? serverType) {
        var books = serverType.HasValue ? 
            await bookService.GetAllAsync(serverType.Value) : 
            await bookService.GetAllAsync();
        return Ok(books.Select(b => new BookResponse(b)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponse>> Get(int id) {
        var book = await bookService.GetBookAsync(id);
        if (book == null) return NotFound();
        return Ok(new BookResponse(book));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest request) {
        Book book = new() {
            Server = request.Server,
            AuthorId = request.AuthorId,
            Metadata = request.Metadata
        };

        await bookService.CreateAsync(book);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] UpdateBookRequest request) {
        Book book = new() {
            Id = request.Id,
            Server = request.Server,
            AuthorId = request.AuthorId,
            Metadata = request.Metadata,
        };

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

public class UserResponse(User user) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = user.Id;

    [JsonPropertyName("username")]
    public string UserName { get; set; } = user.UserName ?? "";

    [JsonPropertyName("email")]
    public string Email { get; set; } = user.Email ?? "";

    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; } = user.PhoneNumber;
}

public class BookResponse(Book book) {
    [JsonPropertyName("id")]
    public int Id { get; set; } = book.Id;

    [JsonPropertyName("server")]
    public ServerType Server { get; set; } = book.Server;

    [JsonPropertyName("author")]
    public UserResponse Author { get; set; } = new(book.Author);

    [JsonPropertyName("metadata")]
    public BusinessLogicLayer.Servers.Books.Book? Metadata { get; set; } = book.Metadata;
}

public class CreateBookRequest {
    [JsonPropertyName("server")]
    public ServerType Server { get; set; }

    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; } = "";

    [JsonPropertyName("metadata")]
    public BusinessLogicLayer.Servers.Books.Book Metadata { get; set; } = null!;
}

public class UpdateBookRequest {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("server")]
    public ServerType Server { get; set; }

    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; } = "";

    [JsonPropertyName("metadata")]
    public BusinessLogicLayer.Servers.Books.Book Metadata { get; set; } = null!;
}