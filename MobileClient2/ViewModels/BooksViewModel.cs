using System.Collections.ObjectModel;
using System.Windows.Input;
using MobileClient2.Models;
using MobileClient2.Services;

namespace MobileClient2.ViewModels;

public class BooksViewModel : BaseViewModel {
    private readonly IBookService _bookService;
    private ObservableCollection<Book> _books;
    private string _searchQuery = string.Empty;
    private bool _isRefreshing;

    public ObservableCollection<Book> Books {
        get => _books;
        set => SetProperty(ref _books, value);
    }

    public string SearchQuery {
        get => _searchQuery;
        set {
            if (SetProperty(ref _searchQuery, value)) {
                FilterBooks(value);
            }
        }
    }

    public bool IsRefreshing {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand CreateBookCommand { get; }

    public BooksViewModel(IBookService bookService) {
        _bookService = bookService;
        _books = new ObservableCollection<Book>();

        RefreshCommand = new Command(async () => await LoadBooksAsync());
        CreateBookCommand = new Command(async () => await Shell.Current.GoToAsync("///book/create"));
    }

    public async Task LoadBooksAsync() {
        try {
            IsRefreshing = true;

            var books = await _bookService.GetBooksAsync();
            Books.Clear();
            foreach (var book in books) {
                Books.Add(book);
            }
        } catch {
            await Shell.Current.DisplayAlert("Error", "Failed to load books", "OK");
        } finally {
            IsRefreshing = false;
        }
    }

    public void FilterBooks(string searchQuery) {
        if (string.IsNullOrWhiteSpace(searchQuery)) {
            LoadBooksAsync();
            return;
        }

        var filteredBooks = Books.Where(b =>
            (b.Metadata?.Title?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (b.Author?.UserName?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false)
        ).ToList();

        Books.Clear();
        foreach (var book in filteredBooks) {
            Books.Add(book);
        }
    }
} 