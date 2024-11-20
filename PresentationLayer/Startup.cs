using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using BusinessLogicLayer.Events;
using Utils.Encryption;
using JokesServiceReference;
using Utils;
using BusinessLogicLayer.Servers.Books;
using Syncfusion.Licensing;

using SteganServices = BusinessLogicLayer.Services.Stegan;
using SteganRepositories = DataAccessLayer.Repositories.Stegan;

using NimbusV1 = DataAccessLayer.Repositories.Nimbus.v1;
using NimbusV2 = DataAccessLayer.Repositories.Nimbus.v2;
using DataAccessLayer.Repositories.Nimbus;
using Utils.Senders;

namespace PresentationLayer {
    public class Startup(IConfiguration configuration) {
        public IConfiguration Configuration { get; } = configuration;

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    config.AddUserSecrets<Startup>();
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });


        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(PathManager.GenerateConnectionString("Main.mdf"))
                    .EnableSensitiveDataLogging());

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILibraryBookRepository, LibraryBookRepository>();
            services.AddScoped<IBookLoanRepository, BookLoanRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IUserSenderRepository, UserSenderRepository>();
            services.AddScoped<ISenderRepository, SenderRepository>();
            services.AddScoped<SteganRepositories.IBookMetadataRepository, SteganRepositories.BookMetadataRepository>();

            // Nimbus repositories
            services.AddScoped<NimbusV1.BookMetadataRepository>();
            services.AddScoped<NimbusV2.BookMetadataRepository>();
            services.AddScoped<NimbusV1.BookChapterRepository>();
            services.AddScoped<NimbusV2.BookChapterRepository>();
            services.AddScoped<INimbusFactory, NimbusFactory>();

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
            services.AddScoped<IUserSenderService, UserSenderService>();
            services.AddScoped<ISenderService, SenderService>();
            services.AddScoped<SteganServices.IBookMetadataService, SteganServices.BookMetadataService>();

            // Register utils
            services.AddTransient<IEncryption, Sha256Encryption>();
            services.AddTransient<IFileUploader, FileUploader>();
            services.AddTransient<ISenderFactory, SenderFactory>();
            services.AddTransient<IAiService, GeminiService>();

            // Register web services
            services.AddScoped(serviceProvider =>
                BooksServerFactory.CreateServer(serviceProvider));
            services.AddTransient(_ => new JokesServicePortTypeClient(
                JokesServicePortTypeClient.EndpointConfiguration.JokesServicePort
            ));

            services.AddHttpClient();

            services.AddControllersWithViews();
            services.AddRazorPages();


            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            EventSubscriber.Subscribe(services.BuildServiceProvider());

            SyncfusionLicenseProvider.RegisterLicense(Configuration["SYNCFUSION_LICENSE_KEY"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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
        }
       
    }
}