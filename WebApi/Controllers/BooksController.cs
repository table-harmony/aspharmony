using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Utils;

using FileIO = System.IO.File;
using BusinessLogicLayer.Servers.Books;

namespace WebApi.Controllers {
    [Route("api/books")]
    [ApiController]
    public class BooksController() : ControllerBase {
        private readonly string filePath = PathManager.GetFilePath(FolderType.Books, "Atlas.json");
            
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id) {
            List<Book> books = await ReadFileAsync();

            Book? book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            
            return Ok(book);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks() {
            List<Book> books = await ReadFileAsync();
            return Ok(books);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] Book newBook) {
            List<Book> books = await ReadFileAsync();

            books.Add(newBook);
            await WriteToFileAsync(books);

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromBody] Book updatedBook) {
            List<Book> books = await ReadFileAsync();

            Book? book = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.ImageUrl = updatedBook.ImageUrl;
            book.Chapters = updatedBook.Chapters;

            await WriteToFileAsync(books);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id) {
            List<Book> books = await ReadFileAsync();

            Book? book = books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            books.Remove(book);
            await WriteToFileAsync(books);

            return NoContent();
        }

        private async Task<List<Book>> ReadFileAsync() {
            if (!FileIO.Exists(filePath))
                return [];

            JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
            string? json = await FileIO.ReadAllTextAsync(filePath);

            BooksWrapper? data = JsonSerializer.Deserialize<BooksWrapper>(json, options);
            return data?.Books ?? [];
        }

        private async Task WriteToFileAsync(List<Book> books) {
            if (!FileIO.Exists(filePath))
                return;

            BooksWrapper wrapper = new() { Books = books };

            await FileIO.WriteAllTextAsync(filePath, JsonSerializer.Serialize(wrapper));
        }
    }

    public class BooksWrapper {
        public required List<Book> Books { get; set; } = [];
    }
}