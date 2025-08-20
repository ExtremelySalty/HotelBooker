using HotelBooker.Application.Interfaces.Repositories;
using HotelBooker.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelBooker.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence
        (
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApiHotelBoooker"))
            );

            services
                .AddScoped<ISeedData, SeedDataRepository>()
                .AddScoped<IHotelRepository, HotelRepository>();

            return services;
        }
    }
}
