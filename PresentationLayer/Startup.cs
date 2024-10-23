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
using DataAccessLayer.Repositories.Nimbus;
using BusinessLogicLayer.Services.Nimbus;
using Syncfusion.Licensing;
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
            services.AddScoped<IBookMetadataRepository, BookMetadataRepository>();
            services.AddScoped<IBookChapterRepository, BookChapterRepository>();
            services.AddScoped<IUserSenderRepository, UserSenderRepository>();
            services.AddScoped<ISenderRepository, SenderRepository>();

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
            services.AddScoped<IBookMetadataService, BookMetadataService>();
            services.AddScoped<IBookChapterService, BookChapterService>();
            services.AddScoped<IUserSenderService, UserSenderService>();
            services.AddScoped<ISenderService, SenderService>();

            // Register utils
            services.AddTransient<IEncryption, Sha256Encryption>();
            services.AddTransient<IFileUploader, FileUploader>();
            services.AddTransient<ISenderFactory, SenderFactory>();

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