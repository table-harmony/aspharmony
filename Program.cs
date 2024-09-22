using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationContext>();

builder.Services.AddControllersWithViews();

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
builder.Services.AddScoped<IBookLoanRepository, BookLoanRepository>();

// Register services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();
builder.Services.AddScoped<IBookLoanService, BookLoanService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// Add this after builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var bookLoanRepo = services.GetRequiredService<IBookLoanRepository>();
    var bookLoanService = services.GetRequiredService<IBookLoanService>();
}