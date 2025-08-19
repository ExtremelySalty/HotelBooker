using HotelBooker.Application;
using HotelBooker.Persistence;

namespace HotelBooker.WebApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureProjects
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddApplication()
                .AddPersistence(configuration);

            return services;
        }

        public static IServiceCollection ConfigureOptions
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            return services;
        }
    }
}
