using System.Collections.ObjectModel;
using System.Windows.Input;
using MobileClient2.Models;
using MobileClient2.Services;

namespace MobileClient2.ViewModels;

public class LibrariesViewModel : BaseViewModel {
    private readonly ILibraryService _libraryService;
    private ObservableCollection<Library> _libraries;
    private string _searchQuery = string.Empty;
    private bool _isRefreshing;

    public ObservableCollection<Library> Libraries {
        get => _libraries;
        set => SetProperty(ref _libraries, value);
    }

    public string SearchQuery {
        get => _searchQuery;
        set {
            if (SetProperty(ref _searchQuery, value)) {
                FilterLibraries(value);
            }
        }
    }

    public bool IsRefreshing {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand CreateLibraryCommand { get; }

    public LibrariesViewModel(ILibraryService libraryService) {
        _libraryService = libraryService;
        _libraries = new ObservableCollection<Library>();

        RefreshCommand = new Command(async () => await LoadLibrariesAsync());
        CreateLibraryCommand = new Command(async () => await Shell.Current.GoToAsync("///libraries/create"));
    }

    public async Task LoadLibrariesAsync() {
        try {
            IsRefreshing = true;

            var libraries = await _libraryService.GetLibrariesAsync();
            Libraries.Clear();
            foreach (var library in libraries) {
                Libraries.Add(library);
            }
        } catch {
            await Shell.Current.DisplayAlert("Error", "Failed to load libraries", "OK");
        } finally {
            IsRefreshing = false;
        }
    }

    public void FilterLibraries(string searchQuery) {
        if (string.IsNullOrWhiteSpace(searchQuery)) {
            LoadLibrariesAsync();
            return;
        }

        var filteredLibraries = Libraries.Where(l =>
            l.Name?.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ?? false
        ).ToList();

        Libraries.Clear();
        foreach (var library in filteredLibraries) {
            Libraries.Add(library);
        }
    }
} 