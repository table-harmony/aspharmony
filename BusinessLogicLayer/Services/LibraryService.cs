using System;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Exceptions;

namespace BusinessLogicLayer.Services {
    public interface ILibraryService {
        Task<IEnumerable<Library>> GetAllAsync();
        Task<Library> GetLibraryAsync(int id);
        Task CreateAsync(string name, string userId);
        Task UpdateAsync(int id, string name);
        Task DeleteAsync(int id);
    }

    public class LibraryService : ILibraryService {
        private readonly ILibraryRepository _libraryRepository;
        private readonly ILibraryMembershipService _membershipService;

        public LibraryService(ILibraryRepository libraryRepository, 
                                ILibraryMembershipService membershipService) {
            _libraryRepository = libraryRepository;
            _membershipService = membershipService;
        }

        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await _libraryRepository.GetAllAsync();
        }

        public async Task<Library> GetLibraryAsync(int id) {
            return await _libraryRepository.GetLibraryAsync(id);
        }

        public async Task CreateAsync(string name, string userId) {
            Library library = new Library {
                Name = name
            };

            await _libraryRepository.CreateAsync(library);
            await _membershipService.CreateAsync(userId, library.Id, MembershipRole.Manager);
        }

        public async Task UpdateAsync(int id, string name) {
            var library = await _libraryRepository.GetLibraryAsync(id);
            if (library == null)
                throw new NotFoundException();
         
            library.Name = name;
            await _libraryRepository.UpdateAsync(library);
        }

        public async Task DeleteAsync(int id) {
            var library = await _libraryRepository.GetLibraryAsync(id);
            if (library == null)
                throw new NotFoundException();

            await _libraryRepository.DeleteAsync(library.Id);
        }
    }
}
