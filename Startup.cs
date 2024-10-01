public void ConfigureServices(IServiceCollection services)
{
    // ... existing service registrations ...

    services.AddScoped<FeedbackService>(sp => 
        new FeedbackService(
            Path.Combine(sp.GetRequiredService<IWebHostEnvironment>().ContentRootPath, "App_Data", "Feedback.xml"),
            sp.GetRequiredService<ApplicationDbContext>()
        ));
}