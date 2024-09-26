using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Utils.Services;
using BusinessLogicLayer.Initiate;

namespace PresentationLayer
{
    public class Startup  {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register Identity services
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

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILibraryService, LibraryService>();
            services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ILibraryBookService, LibraryBookService>();
            services.AddScoped<IBookLoanService, BookLoanService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<INotificationService, NotificationService>();

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
            }
            else {
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
    }
}