using BusinessLogicLayer.Events;
using BusinessLogicLayer.Servers.Books;
using BusinessLogicLayer.Services;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Update the connection string
string path = Path.Combine(Directory.GetCurrentDirectory(),
    "..", "Storage", "App_Data", "Database.mdf");
path = Path.GetFullPath(path);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
connectionString = connectionString.Replace("{path}", path);

// Then configure your DbContext
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString)
        .EnableSensitiveDataLogging());

// Register web service
builder.Services.AddTransient(sp => BooksServerFactory.CreateService(builder.Configuration, sp));

// Register your repositories
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILibraryBookRepository, LibraryBookRepository>();
builder.Services.AddScoped<IBookLoanRepository, BookLoanRepository>();
builder.Services.AddScoped<IBookMetadataRepository, BookMetadataRepository>();
builder.Services.AddScoped<IBookChapterRepository, BookChapterRepository>();

// Register your services
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILibraryBookService, LibraryBookService>();
builder.Services.AddScoped<IBookLoanService, BookLoanService>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IBookMetadataService, BookMetadataService>();
builder.Services.AddScoped<IBookChapterService, BookChapterService>();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaGeneratorOptions.SchemaIdSelector = type => {
        if (type == typeof(DataAccessLayer.Entities.Book))
            return "DataAccessLayer.Book";
        if (type == typeof(BusinessLogicLayer.Servers.Books.Book))
            return "BusinessLogicLayer.Servers.Books.Book";
        return type.FullName;
    };
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
