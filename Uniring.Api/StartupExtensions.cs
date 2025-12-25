using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Uniring.Api.Authentication;
using Uniring.Application;
using Uniring.Domain.Entities.IdentityEntities;
using Uniring.Infrastructure;

namespace Uniring.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(
            this WebApplicationBuilder builder)
        {
            // Add services to the container

            builder.Services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
                    });

            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

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


            // Jwt
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings?.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings?.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        
                        RoleClaimType = ClaimTypes.Role

                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]) &&
                                context.Request.Cookies.TryGetValue("authToken", out string token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization();


            return builder.Build();
        }

        public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app)
        {
            //app.MapIdentityApi<ApplicationUser>();

            //app.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
            //{
            //    await signInManager.SignOutAsync();
            //    return TypedResults.Ok();
            //});

            //app.UseCors("open");
            // AFTER building the app, BEFORE running it
            using var DBscope = app.Services.CreateScope();
            var dbContext = DBscope.ServiceProvider.GetRequiredService<UniringDbContext>();
            dbContext.Database.Migrate(); // Applies pending migrations

            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                
                await DatabaseInitializer.InitializeRolesAsync(roleManager);
                await DatabaseInitializer.InitializeAdminsAsync(userManager, roleManager);
            }

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
