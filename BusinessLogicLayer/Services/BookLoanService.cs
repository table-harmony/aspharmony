using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Exceptions;
using BusinessLogicLayer.Events;

namespace BusinessLogicLayer.Services {

    public interface IBookLoanService
    {
        Task<BookLoan> GetByIdAsync(int id);
        Task<IEnumerable<BookLoan>> GetByUserIdAsync(string userId);
        Task<IEnumerable<BookLoan>> GetByLibraryIdAsync(int libraryId);
        Task CreateLoanAsync(int libraryBookId, string userId, int libraryId, DateTime dueDate);
        Task ReturnBookAsync(int bookLoanId);
        Task<IEnumerable<BookLoan>> GetPastLoansByLibraryBookIdAsync(int libraryBookId);
        Task<BookLoan> GetCurrentLoanByLibraryBookIdAsync(int libraryBookId);
        event EventHandler<BookBorrowedEventArgs> BookBorrowed;
    }

    public class BookLoanService : IBookLoanService
    {
        private readonly IBookLoanRepository _bookLoanRepository;
        private readonly ILibraryMembershipRepository _libraryMembershipRepository;
        private readonly ILibraryService _libraryService;

        public event EventHandler<BookBorrowedEventArgs> BookBorrowed;

        public BookLoanService(IBookLoanRepository bookLoanRepository, ILibraryMembershipRepository libraryMembershipRepository, ILibraryService libraryService)
        {
            _bookLoanRepository = bookLoanRepository;
            _libraryMembershipRepository = libraryMembershipRepository;
            _libraryService = libraryService;
        }

        public async Task<BookLoan> GetByIdAsync(int id)
        {
            return await _bookLoanRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BookLoan>> GetByUserIdAsync(string userId)
        {
            return await _bookLoanRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<BookLoan>> GetByLibraryIdAsync(int libraryId)
        {
            return await _bookLoanRepository.GetByLibraryIdAsync(libraryId);
        }

        public async Task CreateLoanAsync(int libraryBookId, string userId, int libraryId, DateTime dueDate)
        {
            var membership = await _libraryMembershipRepository.GetByLibraryAndUserIdAsync(libraryId, userId);
            if (membership == null) {
                throw new PublicException("User is not a member of this library");
            }

            var currentLoan = await _bookLoanRepository.GetCurrentLoanByLibraryBookIdAsync(libraryBookId);
            if (currentLoan != null) {
                throw new PublicException("This book is already on loan");
            }

            var bookLoan = new BookLoan
            {
                LibraryBookId = libraryBookId,
                UserId = userId,
                LibraryId = libraryId,
                LoanDate = DateTime.Now,
                DueDate = dueDate
            };

            await _bookLoanRepository.CreateAsync(bookLoan);

            var libraryBook = await _libraryService.GetLibraryBookByIdAsync(libraryBookId);
            BookLoanEvents.OnBookBorrowed(libraryId, userId, libraryBook.Book.Title);
        }

        public async Task ReturnBookAsync(int bookLoanId)
        {
            var bookLoan = await _bookLoanRepository.GetByIdAsync(bookLoanId);
            if (bookLoan == null)
            {
                throw new NotFoundException();
            }

            bookLoan.ReturnDate = DateTime.Now;
            await _bookLoanRepository.UpdateAsync(bookLoan);
        }

        public async Task<IEnumerable<BookLoan>> GetPastLoansByLibraryBookIdAsync(int libraryBookId)
        {
            return await _bookLoanRepository.GetPastLoansByLibraryBookIdAsync(libraryBookId);
        }

        public async Task<BookLoan> GetCurrentLoanByLibraryBookIdAsync(int libraryBookId)
        {
            return await _bookLoanRepository.GetCurrentLoanByLibraryBookIdAsync(libraryBookId);
        }
    }

    public class BookBorrowedEventArgs : EventArgs
    {
        public int LibraryId { get; set; }
        public string UserId { get; set; }
        public string BookTitle { get; set; }
    }
}