using Microsoft.Extensions.DependencyInjection;

namespace HotelBooker.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
                });

            return services;
        }
    }
}
