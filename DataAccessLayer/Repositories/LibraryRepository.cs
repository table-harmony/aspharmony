﻿using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface ILibraryRepository {
        Task<Library?> GetLibraryAsync(int id);
        Task<IEnumerable<Library>> GetAllAsync();
        Task CreateAsync(Library library);
        Task UpdateAsync(Library library);
        Task DeleteAsync(int id);
    }

    public class LibraryRepository(ApplicationContext context) : ILibraryRepository {
        public async Task<Library?> GetLibraryAsync(int id) {
            return await context.Libraries
                .AsNoTracking()
                .Include(l => l.Memberships)
                .Include(l => l.Books)
                    .ThenInclude(lb => lb.Book)
                    .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Library>> GetAllAsync() {
            return await context.Libraries.ToListAsync();
        }

        public async Task CreateAsync(Library library) {
            await context.Libraries.AddAsync(library);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Library library) {
            context.Libraries.Update(library);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            Library? library = await GetLibraryAsync(id);

            if (library == null)
                throw new NotFoundException();

            context.Libraries.Remove(library);
            await context.SaveChangesAsync();
        }
    }
}
