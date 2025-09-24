using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Uniring.Application.Interfaces;
using Uniring.Application.Interfaces.Repositories;
using Uniring.Application.Services;
using Uniring.Domain.Entities.IdentityEntities;
using Uniring.Infrastructure;
using Uniring.Infrastructure.Repositories;

namespace Uniring.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllers();

            //builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            //builder.Services.AddPersistenceServices(builder.Configuration);
            //builder.Services.AddIdentityServices(builder.Configuration);

            //builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();

            //builder.Services.AddHttpContextAccessor();

            //builder.Services.AddCors(
            //    options => options.AddPolicy(
            //        "open",
            //        policy => policy.WithOrigins([builder.Configuration["ApiUrl"] ?? "https://localhost:7081",
            //            builder.Configuration["BlazorUrl"] ?? "https://localhost:7080"])
            //.AllowAnyMethod()
            //.SetIsOriginAllowed(pol => true)
            //.AllowAnyHeader()
            //.AllowCredentials()));

            // TEMP ---------------------

            builder.Services.AddScoped<IRingRepository, RingRepository>();
            builder.Services.AddScoped<IRingService, RingService>();

            // --------------------------

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseCustomExceptionHandler();
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseCors("DevCors"); // <-- IMPORTANT: enable cors before MapControllers

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
