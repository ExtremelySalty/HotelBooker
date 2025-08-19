﻿using Microsoft.Extensions.Configuration;
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
            return services;
        }
    }
}
