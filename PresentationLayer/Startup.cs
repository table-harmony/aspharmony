using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Utils.Services;
using BusinessLogicLayer.Initiate;
using BookServiceReference;
using BusinessLogicLayer.Events;
using Utils.Encryption;
using AspHarmonyServiceReference;
using System.ServiceModel;

namespace PresentationLayer
{
    public class Startup(IConfiguration configuration) {
        public IConfiguration Configuration { get; } = configuration;

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        public void ConfigureServices(IServiceCollection services) {
            // Add the DbContext to the services
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(GetConnectionString())
                    .EnableSensitiveDataLogging());

            // Register Identity services
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // Register the SOAP clients
            services.AddScoped(_ =>
                new BookServiceSoapClient(BookServiceSoapClient.EndpointConfiguration.BookServiceSoap));
            services.AddScoped(_ => {
                BasicHttpBinding binding = new(BasicHttpSecurityMode.Transport);
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

                EndpointAddress endpoint = new("https://aspharmony-production.up.railway.app/service");

                AspHarmonyPortTypeClient client = new(binding, endpoint);

                return client;
            });

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILibraryBookRepository, LibraryBookRepository>();
            services.AddScoped<IBookLoanRepository, BookLoanRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILibraryBookService, LibraryBookService>();
            services.AddScoped<IBookLoanService, BookLoanService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEventTracker, EventTracker>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            // Register utils
            services.AddScoped<IEncryption, Sha256Encryption>();
            services.AddScoped<IFileUploader, FileUploader>();

            // Add HttpClient
            services.AddHttpClient();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            EventSubscriber.Subscribe(services.BuildServiceProvider());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

           RoleInitializer.InitializeAsync(app.ApplicationServices).Wait();
        }
        
        private string GetConnectionString() {
            string path = Path.Combine(Directory.GetCurrentDirectory(), 
                "..", "Storage", "App_Data", "Database.mdf");
            path = Path.GetFullPath(path);

            string connectionString = Configuration.GetConnectionString("DefaultConnection")!;
            return connectionString.Replace("{path}", path);
        }
    }
}