using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController(ILibraryService libraryService) : ControllerBase {

        // GET: api/Library
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetAllLibraries() {
            var libraries = await libraryService.GetAllAsync();
            return Ok(libraries);
        }

        // GET: api/Library/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetLibrary(int id) {
            var library = await libraryService.GetLibraryAsync(id);
            if (library == null) {
                return NotFound();
            }
            return Ok(library);
        }

        // POST: api/Library
        [HttpPost]
        public async Task<ActionResult<Library>> CreateLibrary(Library library) {
            await libraryService.CreateAsync(library);
            return CreatedAtAction(nameof(GetLibrary), new { id = library.Id }, library);
        }

        // PUT: api/Library/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLibrary(Library library) {
            try {
                await libraryService.UpdateAsync(library);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        // DELETE: api/Library/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibrary(int id) {
            try {
                await libraryService.DeleteAsync(id);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}