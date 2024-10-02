using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase {

        private readonly ILogger<LibraryController> _logger;
        private readonly ILibraryService _libraryService;

        public LibraryController(ILogger<LibraryController> logger,
                                    ILibraryService libraryService) {
            _logger = logger;
            _libraryService = libraryService;
        }

        [HttpGet(Name = "GetLibraries")]
        public async Task<IEnumerable<Library>> Get() {
            return await _libraryService.GetAllAsync();
        }

        [HttpGet(Name = "GetLibrary")]
        public async Task<Library> Get(int id) {
            return await _libraryService.GetLibraryAsync(id);
        }

        [HttpPost(Name = "CreateLibrary")]
        public async void Post(string name) {
            Library library = new() { Name = name };
            await _libraryService.CreateAsync(library);
        }

        [HttpDelete(Name = "DeleteLibrary")]
        public async void Delete(int id) {
            await _libraryService.DeleteAsync(id);
        }

        [HttpPatch(Name = "UpdateLibrary")]
        public async void Patch(int id, string name) {
            await _libraryService.UpdateAsync(id, name);
        }

    }
}