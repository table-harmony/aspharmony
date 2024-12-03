using MobileClient2.Services;
using MobileClient2.Views;

namespace MobileClient2;

public partial class AppShell : Shell {
    private readonly IAuthService _authService;
    private bool _isAuthenticated;

    public bool IsAuthenticated {
        get => _isAuthenticated;
        set {
            _isAuthenticated = value;
            OnPropertyChanged();
        }
    }

    public AppShell(IAuthService authService) {
        InitializeComponent();
        _authService = authService;
        BindingContext = this;

        Routing.RegisterRoute("books", typeof(BooksPage));
        Routing.RegisterRoute("libraries", typeof(LibrariesPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("register", typeof(RegisterPage));

        IsAuthenticated = _authService.IsAuthenticated();
        if (!IsAuthenticated) {
            Dispatcher.Dispatch(async () => {
                await Shell.Current.GoToAsync("///login");
            });
        }
    }

    protected override void OnNavigating(ShellNavigatingEventArgs args) {
        base.OnNavigating(args);

        var targetRoute = args.Target.Location.OriginalString;

        bool requiresAuth = targetRoute.Contains("books") || targetRoute.Contains("libraries");
        bool isAuthRoute = targetRoute.Contains("login") || targetRoute.Contains("register");

        if (requiresAuth && !_authService.IsAuthenticated()) {
            args.Cancel();
            Dispatcher.Dispatch(async () => {
                await Shell.Current.GoToAsync("///login");
            });
        } else if (isAuthRoute && _authService.IsAuthenticated()) {
            args.Cancel();
            Dispatcher.Dispatch(async () => {
                await Shell.Current.GoToAsync("///main");
            });
        }
    }

    public void UpdateAuthenticationState(bool isAuthenticated) {
        IsAuthenticated = isAuthenticated;
    }
}
