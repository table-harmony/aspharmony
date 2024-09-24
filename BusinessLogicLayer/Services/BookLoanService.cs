using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Exceptions;
using BusinessLogicLayer.Events;

namespace BusinessLogicLayer.Services {

    public interface IBookLoanService {
        Task<BookLoan> GetBookLoanAsync(int id);
        Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate);
        Task ReturnBookAsync(int id);
        IEnumerable<BookLoan> GetBookLoans(int bookId);
        Task UpdateAsync(BookLoan loan);
        Task<BookLoan> GetCurrentBookLoanAsync(int bookId);
    }

    public class BookLoanService : IBookLoanService {
        private readonly IBookLoanRepository _loanRepository;

        public BookLoanService(IBookLoanRepository loanRepository) {
            _loanRepository = loanRepository;
        }

        public async Task<BookLoan> GetBookLoanAsync(int id) {
            return await _loanRepository.GetBookLoanAsync(id);
        }

        public IEnumerable<BookLoan> GetBookLoans(int bookId) {
            return _loanRepository.GetBookLoans(bookId);
        }

        public async Task CreateAsync(int bookId, int libraryMembershipId, DateTime dueDate) {
            BookLoan currentLoan = await GetCurrentBookLoanAsync(bookId);
            if (currentLoan != null)
                throw new AuthorizationException();

            BookLoan loan = new BookLoan {
                LibraryBookId = currentLoan.LibraryBookId,
                LibraryMembershipId = libraryMembershipId,
                LoanDate = DateTime.Now,
                DueDate = dueDate
            };

            await _loanRepository.CreateAsync(loan);
        }

        public async Task UpdateAsync(BookLoan loan) {
            await _loanRepository.UpdateAsync(loan);
        }

        public async Task<BookLoan> GetCurrentBookLoanAsync(int bookId) {
            return await _loanRepository.GetCurrentLoanAsync(bookId);
        }

        public async Task ReturnBookAsync(int id) {
            BookLoan loan = await GetBookLoanAsync(id);
            if (loan == null)
                throw new NotFoundException();

            loan.ReturnDate = DateTime.Now;
            await UpdateAsync(loan);
        }
    }
}