using System.Windows.Input;
using MobileClient2.Services;

namespace MobileClient2.ViewModels;

public class LoginViewModel : BaseViewModel {
    private readonly IAuthService _authService;
    private string _email;
    private string _password;
    private bool _isBusy;

    public string Email {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public bool IsBusy {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService) {
        _authService = authService;
        LoginCommand = new Command(async () => await LoginAsync());
    }

    private async Task LoginAsync() {
        if (IsBusy) return;
        IsBusy = true;

        try {
            var user = await _authService.LoginAsync(Email, Password);
            if (user != null) {
                ((AppShell)Shell.Current).UpdateAuthenticationState(true);
                await Shell.Current.GoToAsync("//home");
            }
        } catch (Exception ex) {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        } finally {
            IsBusy = false;
        }
    }
}
