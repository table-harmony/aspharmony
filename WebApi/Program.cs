using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Events;
using Utils.Encryption;
using Utils;
using BusinessLogicLayer.Servers.Books;
using Microsoft.OpenApi.Models;

using SteganServices = BusinessLogicLayer.Services.Stegan;
using SteganRepositories = DataAccessLayer.Repositories.Stegan;
using NimbusV1 = DataAccessLayer.Repositories.Nimbus.v1;
using NimbusV2 = DataAccessLayer.Repositories.Nimbus.v2;
using DataAccessLayer.Repositories.Nimbus;
using Utils.Senders;

var builder = WebApplication.CreateBuilder(args);

// Configure database
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(PathManager.GenerateConnectionString("Main.mdf"))
        .EnableSensitiveDataLogging());

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ILibraryMembershipRepository, LibraryMembershipRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILibraryBookRepository, LibraryBookRepository>();
builder.Services.AddScoped<IBookLoanRepository, BookLoanRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IUserSenderRepository, UserSenderRepository>();
builder.Services.AddScoped<ISenderRepository, SenderRepository>();
builder.Services.AddScoped<SteganRepositories.IBookMetadataRepository, SteganRepositories.BookMetadataRepository>();

// Nimbus repositories
builder.Services.AddScoped<NimbusV1.BookMetadataRepository>();
builder.Services.AddScoped<NimbusV2.BookMetadataRepository>();
builder.Services.AddScoped<NimbusV1.BookChapterRepository>();
builder.Services.AddScoped<NimbusV2.BookChapterRepository>();
builder.Services.AddScoped<INimbusFactory, NimbusFactory>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILibraryService, LibraryService>();
builder.Services.AddScoped<ILibraryMembershipService, LibraryMembershipService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILibraryBookService, LibraryBookService>();
builder.Services.AddScoped<IBookLoanService, BookLoanService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IUserSenderService, UserSenderService>();
builder.Services.AddScoped<ISenderService, SenderService>();
builder.Services.AddScoped<SteganServices.IBookMetadataService, SteganServices.BookMetadataService>();

// Register utils
builder.Services.AddTransient<IEncryption, Sha256Encryption>();
builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddTransient<ISenderFactory, SenderFactory>();
builder.Services.AddTransient<ITextModelService, GeminiService>();
builder.Services.AddTransient<IImageModelService, StabilityService>();
builder.Services.AddTransient<ITextToSpeechService, ElevenLabsService>();

// Register BooksServer factory
builder.Services.AddScoped(serviceProvider =>
    BooksServerFactory.CreateServer(serviceProvider));

// Configure Swagger
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "AspHarmony API",
        Version = "v1",
        Description = "API for AspHarmony applications"
    });

    c.CustomSchemaIds(type => type.FullName);

    c.SchemaGeneratorOptions.SchemaIdSelector = type => {
        if (type == typeof(DataAccessLayer.Entities.Book))
            return "DataAccessLayer.Book";
        if (type == typeof(BusinessLogicLayer.Servers.Books.Book))
            return "BusinessLogicLayer.Servers.Books.Book";
        return type.FullName;
    };
});

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();