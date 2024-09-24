using System;

namespace BusinessLogicLayer.Events
{
    public class BookBorrowedEventArgs : EventArgs
    {
        public int LibraryId { get; set; }
        public string UserId { get; set; }
        public string BookTitle { get; set; }
    }

    public static class BookLoanEvents
    {
        public static event EventHandler<BookBorrowedEventArgs> BookBorrowed;

        public static void OnBookBorrowed(int libraryId, string userId, string bookTitle)
        {
            BookBorrowed?.Invoke(null, new BookBorrowedEventArgs { LibraryId = libraryId, UserId = userId, BookTitle = bookTitle });
        }
    }
}