using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;
using BusinessLogicLayer.Services;
using System.Text.Json.Serialization;

namespace WebApi.Controllers;

[Route("api/libraries")]
[ApiController]
public class LibraryController(
    ILibraryService libraryService,
    ILibraryBookService libraryBookService,
    ILibraryMembershipService membershipService,
    IBookService bookService) : ControllerBase {

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibraryResponse>>> GetAll() {
        var libraries = await libraryService.GetAllAsync();
        return Ok(libraries.Select(l => new LibraryResponse(l)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LibraryResponse>> Get(int id) {
        var library = await libraryService.GetLibraryAsync(id);
        if (library == null) return NotFound();

        var books = await libraryBookService.GetLibraryBooksAsync(id);
        var members = membershipService.GetLibraryMembers(id);

        var response = new LibraryResponse(library) {
            Books = books.Select(b => new LibraryBookResponse(b)).ToList(),
            Members = members.Select(m => new MembershipResponse(m)).ToList()
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<LibraryResponse>> Create([FromBody] CreateLibraryRequest request) {
        Library library = new() {
            Name = request.Name,
            AllowCopies = request.AllowCopies
        };

        await libraryService.CreateAsync(library);
        return CreatedAtAction(nameof(Get), new { id = library.Id }, new LibraryResponse(library));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLibraryRequest request) {
        if (id != request.Id) return BadRequest();

        Library library = new() {
            Id = request.Id,
            Name = request.Name,
            AllowCopies = request.AllowCopies
        };

        await libraryService.UpdateAsync(library);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        await libraryService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{libraryId}/books")]
    public async Task<IActionResult> AddBook(int libraryId, [FromBody] AddBookRequest request) {
        await libraryBookService.CreateAsync(libraryId, request.BookId);
        return NoContent();
    }

    [HttpDelete("{libraryId}/books/{bookId}")]
    public async Task<IActionResult> RemoveBook(int libraryId, int bookId) {
        var libraryBooks = await libraryBookService.GetLibraryBooksAsync(libraryId, bookId);
        var libraryBook = libraryBooks.FirstOrDefault();
        if (libraryBook == null) return NotFound();

        await libraryBookService.DeleteAsync(libraryBook.Id);
        return NoContent();
    }

    [HttpPost("{libraryId}/members")]
    public async Task<ActionResult<MembershipResponse>> JoinLibrary(int libraryId, [FromBody] JoinLibraryRequest request) {
        var membership = new LibraryMembership {
            LibraryId = libraryId,
            UserId = request.UserId,
            Role = request.Role
        };

        await membershipService.CreateAsync(membership);
        return CreatedAtAction(nameof(Get), new { id = libraryId }, new MembershipResponse(membership));
    }

    [HttpDelete("{libraryId}/members/{userId}")]
    public async Task<IActionResult> RemoveMember(int libraryId, string userId) {
        await membershipService.DeleteAsync(libraryId, userId);
        return NoContent();
    }

    [HttpPost("{libraryId}/members/{userId}/promote")]
    public async Task<IActionResult> PromoteMember(int libraryId, string userId) {
        var membership = await membershipService.GetMembershipAsync(libraryId, userId);
        if (membership == null) return NotFound();

        membership.Role = MembershipRole.Manager;
        await membershipService.UpdateAsync(membership);
        return NoContent();
    }

    [HttpPost("{libraryId}/members/{userId}/demote")]
    public async Task<IActionResult> DemoteMember(int libraryId, string userId) {
        var membership = await membershipService.GetMembershipAsync(libraryId, userId);
        if (membership == null) return NotFound();

        membership.Role = MembershipRole.Member;
        await membershipService.UpdateAsync(membership);
        return NoContent();
    }
}

public class LibraryResponse(Library library) {
    [JsonPropertyName("id")]
    public int Id { get; set; } = library.Id;

    [JsonPropertyName("name")]
    public string Name { get; set; } = library.Name;

    [JsonPropertyName("allow_copies")]
    public bool AllowCopies { get; set; } = library.AllowCopies;

    [JsonPropertyName("books")]
    public List<LibraryBookResponse> Books { get; set; } = [];

    [JsonPropertyName("members")]
    public List<MembershipResponse> Members { get; set; } = [];
}

public class LibraryBookResponse(LibraryBook libraryBook) {
    [JsonPropertyName("id")]
    public int Id { get; set; } = libraryBook.Id;

    [JsonPropertyName("book_id")]
    public int BookId { get; set; } = libraryBook.BookId;

    [JsonPropertyName("book")]
    public BookResponse? Book { get; set; } = libraryBook.Book != null ? new BookResponse((BusinessLogicLayer.Services.Book)libraryBook.Book) : null;
}

public class MembershipResponse(LibraryMembership membership) {
    [JsonPropertyName("id")]
    public int Id { get; set; } = membership.Id;

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MembershipRole Role { get; set; } = membership.Role;

    [JsonPropertyName("user")]
    public UserResponse? User { get; set; } = membership.User != null ? new UserResponse(membership.User) : null;
}

public class CreateLibraryRequest {
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("allow_copies")]
    public bool AllowCopies { get; set; }
}

public class UpdateLibraryRequest {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("allow_copies")]
    public bool AllowCopies { get; set; }
}

public class AddBookRequest {
    [JsonPropertyName("book_id")]
    public int BookId { get; set; }
}

public class JoinLibraryRequest {
    [JsonPropertyName("user_id")]
    public required string UserId { get; set; }

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required MembershipRole Role { get; set; } = MembershipRole.Member;
}