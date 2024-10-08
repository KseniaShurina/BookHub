﻿using BookHub.Application.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Application.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapperConfigurations(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthorProfile).Assembly);
            services.AddAutoMapper(typeof(BookProfile).Assembly);
            services.AddAutoMapper(typeof(UserProfile).Assembly);
            services.AddAutoMapper(typeof(UserExternalLoginProviderProfile).Assembly);

            return services;
        }
    }
}
