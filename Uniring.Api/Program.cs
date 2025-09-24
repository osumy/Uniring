using Uniring.Api;

var builder = WebApplication.CreateBuilder(args);

// add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("https://localhost:5011") // App origin (use https port from launchSettings)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

app.Run();
