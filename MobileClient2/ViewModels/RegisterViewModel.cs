using System.Windows.Input;
using MobileClient2.Services;

namespace MobileClient2.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _email;
    private string _password;
    private string _confirmPassword;
    private bool _isBusy;

    public string Email {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ConfirmPassword {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public bool IsBusy {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }

    public RegisterViewModel(IAuthService authService) {
        _authService = authService;
        RegisterCommand = new Command(async () => await RegisterAsync());
        NavigateToLoginCommand = new Command(async () => await Shell.Current.GoToAsync("///login"));
    }

    private async Task RegisterAsync() {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Email) || 
            string.IsNullOrWhiteSpace(Password) || 
            string.IsNullOrWhiteSpace(ConfirmPassword)) {
            await Shell.Current.DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        if (Password != ConfirmPassword) {
            await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        try {
            IsBusy = true;
            var user = await _authService.RegisterAsync(Email, Password);
            await Shell.Current.GoToAsync("///main");
        }
        catch {
            await Shell.Current.DisplayAlert("Error", "Registration failed", "OK");
        }
        finally {
            IsBusy = false;
        }
    }
} 