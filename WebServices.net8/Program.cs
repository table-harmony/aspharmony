using SoapCore;
using WebServices.net8.Services.Books;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the SOAP service
builder.Services.AddScoped<IBooksService, BooksService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.UseSoapEndpoint<IBooksService>("/BooksService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
    endpoints.MapControllers();
});

app.Run();
