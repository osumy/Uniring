using Uniring.Application;
using Uniring.Infrastructure;

namespace Uniring.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            // Add services to the container

            builder.Services.AddControllers();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

            //builder.Services.AddHttpContextAccessor();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:7208") // allowed origin
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            //app.MapIdentityApi<ApplicationUser>();

            //app.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
            //{
            //    await signInManager.SignOutAsync();
            //    return TypedResults.Ok();
            //});

            //app.UseCors("open");

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseCustomExceptionHandler();
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseCors("FrontendPolicy");            // <-- IMPORTANT: enable cors before MapControllers

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
