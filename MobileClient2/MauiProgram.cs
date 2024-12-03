using Microsoft.Extensions.Logging;
using MobileClient2.Services;
using MobileClient2.ViewModels;
using MobileClient2.Views;

namespace MobileClient2;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<AppShell>();
        
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<BooksPage>();
        builder.Services.AddTransient<LibrariesPage>();

        builder.Services.AddTransient<BooksViewModel>();
        builder.Services.AddTransient<LibrariesViewModel>();
        builder.Services.AddSingleton<IBookService, BookService>();
        builder.Services.AddSingleton<ILibraryService, LibraryService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
