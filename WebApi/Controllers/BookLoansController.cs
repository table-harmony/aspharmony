using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/book-loans")]
[ApiController]
public class BookLoansController(IBookLoanService bookLoanService) : ControllerBase {
    [HttpGet("{id}")]
    public async Task<ActionResult<BookLoan>> Get(int id) {
        var loan = await bookLoanService.GetBookLoanAsync(id);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    [HttpGet("book/{bookId}")]
    public ActionResult<IEnumerable<BookLoan>> GetBookLoans(int bookId) {
        var loans = bookLoanService.GetBookLoans(bookId);
        return Ok(loans);
    }

    [HttpPost]
    public async Task<ActionResult<BookLoan>> Create([FromBody] CreateBookLoanRequest request) {
        await bookLoanService.CreateAsync(
            request.LibraryBookId,
            request.LibraryMembershipId,
            request.DueDate
        );
        return Ok();
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> ReturnBook(int id) {
        await bookLoanService.ReturnBookAsync(id);
        return NoContent();
    }

    [HttpGet("{libraryBookId}/current")]
    public async Task<ActionResult<BookLoan>> GetCurrentLoan(int libraryBookId) {
        var loan = await bookLoanService.GetCurrentBookLoanAsync(libraryBookId);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    [HttpGet("{libraryBookId}/active/{membershipId}")]
    public async Task<ActionResult<BookLoan>> GetActiveLoanOrRequest(int libraryBookId, int membershipId) {
        var loan = await bookLoanService.GetActiveLoanOrRequestAsync(libraryBookId, membershipId);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    [HttpPost("request")]
    public async Task<IActionResult> CreateRequest([FromBody] CreateLoanRequestRequest request) {
        await bookLoanService.CreateRequestAsync(request.LibraryBookId, request.MembershipId);
        return Ok();
    }

    [HttpGet("{libraryBookId}/queue-position/{membershipId}")]
    public async Task<ActionResult<int>> GetQueuePosition(int libraryBookId, int membershipId) {
        var position = await bookLoanService.GetQueuePositionAsync(libraryBookId, membershipId);
        return Ok(position);
    }

    [HttpGet("{libraryBookId}/queue")]
    public async Task<ActionResult<IEnumerable<BookLoan>>> GetQueue(int libraryBookId) {
        var queue = await bookLoanService.GetQueueAsync(libraryBookId);
        return Ok(queue);
    }

    [HttpGet("{libraryBookId}/active")]
    public async Task<ActionResult<BookLoan>> GetActiveLoan(int libraryBookId) {
        var loan = await bookLoanService.GetActiveLoanAsync(libraryBookId);
        if (loan == null) return NotFound();
        return Ok(loan);
    }
}

public class CreateBookLoanRequest {
    public int LibraryBookId { get; set; }
    public int LibraryMembershipId { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateLoanRequestRequest {
    public int LibraryBookId { get; set; }
    public int MembershipId { get; set; }
} 