using Microsoft.Extensions.DependencyInjection;
using Uniring.Application.Interfaces;
using Uniring.Application.Services;

namespace Uniring.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRingService, RingService>();
            services.AddScoped<IMediaService, MediaService>();

            return services;
        }
    }
}
