﻿using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Utils.Exceptions;
using System.Collections.Generic;

namespace DataAccessLayer.Repositories {
    public interface ILibraryMembershipRepository {
        Task CreateAsync(LibraryMembership membership);
        Task<LibraryMembership> GetUserMembershipsAsync(string userId);
        Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId);
        Task<LibraryMembership> GetMembershipAsync(int id);
        Task UpdateAsync(LibraryMembership membership);
        Task DeleteAsync(int id);
        IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId);
        Task DeleteAsync(int libraryId, string userId);
    }

    public class LibraryMembershipRepository : ILibraryMembershipRepository {
        private readonly ApplicationContext _context;
        private readonly IBookLoanRepository _bookLoanRepository;

        public LibraryMembershipRepository(ApplicationContext context, 
                                            IBookLoanRepository bookLoanRepository) {
            _context = context;
            _bookLoanRepository = bookLoanRepository;   
        }

        public async Task CreateAsync(LibraryMembership membership) {
            await _context.LibraryMemberships.AddAsync(membership);
            await _context.SaveChangesAsync();
        }

        public async Task<LibraryMembership> GetUserMembershipsAsync(string userId) {
            return await _context.LibraryMemberships
                .Include(membership => membership.Library)
                .Include(membership => membership.User)
                .FirstOrDefaultAsync(membership => membership.UserId == userId);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int libraryId, string userId) {
            return await _context.LibraryMemberships
                .FirstOrDefaultAsync(m => m.LibraryId == libraryId && m.UserId == userId);
        }

        public async Task<LibraryMembership> GetMembershipAsync(int id) {
            return await _context.LibraryMemberships
                .Include(membership => membership.Library)
                .Include(membership => membership.User)
                .FirstOrDefaultAsync(membership => membership.Id == id);
        }

        public async Task UpdateAsync(LibraryMembership membership) {
            _context.LibraryMemberships.Update(membership);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id) {
            var transaction = _context.Database.BeginTransaction();

            LibraryMembership membership = await GetMembershipAsync(id);

            if (membership == null)
                throw new NotFoundException();

            IEnumerable<BookLoan> loans = _bookLoanRepository.GetMemberLoans(id);

            if (loans.Any())
                _context.BookLoans.RemoveRange(membership.BookLoans);

            _context.LibraryMemberships.Remove(membership);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }


        public IEnumerable<LibraryMembership> GetLibraryMembers(int libraryId) {
            return _context.LibraryMemberships
                .Include(membership => membership.User)
                .Where(membership => membership.LibraryId == libraryId)
                .ToList();
        }

        public async Task DeleteAsync(int libraryId, string userId) {
            var transaction = _context.Database.BeginTransaction();

            LibraryMembership membership = await GetMembershipAsync(libraryId, userId);

            if (membership == null)
                throw new NotFoundException();

            IEnumerable<BookLoan> loans = _bookLoanRepository.GetMemberLoans(membership.Id);
            
            if (loans.Any())
                _context.BookLoans.RemoveRange(membership.BookLoans);
            
            _context.LibraryMemberships.Remove(membership);
            await _context.SaveChangesAsync();

            transaction.Commit();
        }
    }

}
