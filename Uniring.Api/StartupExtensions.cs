using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

            builder.Services.AddControllers();

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
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
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

            // Seed roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                await UniringDbContext.InitializeRolesAsync(roleManager);
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
