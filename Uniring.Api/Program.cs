using Uniring.Api;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to allow larger request bodies for file uploads
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 200 MB
});

var app = builder.ConfigureServices();
await app.ConfigurePipelineAsync();

app.Run();
