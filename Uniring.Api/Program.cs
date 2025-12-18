using Uniring.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices();
await app.ConfigurePipelineAsync();

app.Run();
