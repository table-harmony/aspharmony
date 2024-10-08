﻿using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Utils.Exceptions;

namespace DataAccessLayer.Repositories {
    public interface IBookRepository {
        Task<Book?> GetBookAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> CreateAsync(Book book);
        Task DeleteAsync(int id);
        IDbContextTransaction BeginTransaction();
    }

    public class BookRepository(ApplicationContext context) : IBookRepository {
        public async Task<Book?> GetBookAsync(int id) {
            return await context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllAsync() {
            return await context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<Book> CreateAsync(Book book) {
            await context.Books.AddAsync(book);
            await context.SaveChangesAsync();

            return book;
        }

        public async Task DeleteAsync(int id) {
            Book? book = await GetBookAsync(id);

            if (book == null)
                throw new NotFoundException();

            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction() {
            return context.Database.BeginTransaction();
        }
    }

}
