namespace MobileClient2 {
    public partial class App : Application {
        public App(IServiceProvider services) {
            InitializeComponent();
            MainPage = services.GetRequiredService<AppShell>();
        }
    }
}
