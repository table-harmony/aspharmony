using System.Windows.Input;
using MobileClient2.Services;

namespace MobileClient2.ViewModels;

public class HomeViewModel : BaseViewModel {
    private readonly IAuthService _authService;
    private bool _isAuthenticated;

    public bool IsAuthenticated {
        get => _isAuthenticated;
        set {
            if (SetProperty(ref _isAuthenticated, value)) {
                OnPropertyChanged(nameof(IsNotAuthenticated));
                ((Command)NavigateToBooksCommand).ChangeCanExecute();
                ((Command)NavigateToLibrariesCommand).ChangeCanExecute();
                ((Command)NavigateToLoginCommand).ChangeCanExecute();
                ((Command)NavigateToRegisterCommand).ChangeCanExecute();
                ((Command)LogoutCommand).ChangeCanExecute();
            }
        }
    }

    public bool IsNotAuthenticated => !IsAuthenticated;

    public ICommand NavigateToBooksCommand { get; }
    public ICommand NavigateToLibrariesCommand { get; }
    public ICommand NavigateToLoginCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }
    public ICommand LogoutCommand { get; }

    public HomeViewModel(IAuthService authService) {
        _authService = authService;

        NavigateToBooksCommand = new Command(
            async () => await Shell.Current.GoToAsync("///books"),
            () => IsAuthenticated);

        NavigateToLibrariesCommand = new Command(
            async () => await Shell.Current.GoToAsync("///libraries"),
            () => IsAuthenticated);

        NavigateToLoginCommand = new Command(
            async () => await Shell.Current.GoToAsync("///login"),
            () => !IsAuthenticated);

        NavigateToRegisterCommand = new Command(
            async () => await Shell.Current.GoToAsync("///register"),
            () => !IsAuthenticated);

        LogoutCommand = new Command(
            async () => await LogoutAsync(),
            () => IsAuthenticated);

        IsAuthenticated = _authService.IsAuthenticated();
        ((AppShell)Shell.Current).UpdateAuthenticationState(IsAuthenticated);
    }

    private async Task LogoutAsync() {
        await _authService.LogoutAsync();
        ((AppShell)Shell.Current).UpdateAuthenticationState(false);
        await Shell.Current.GoToAsync("///login");
    }

    public void UpdateAuthenticationState() {
        IsAuthenticated = _authService.IsAuthenticated();
    }
} 