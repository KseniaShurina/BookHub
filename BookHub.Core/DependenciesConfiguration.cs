using BookHub.Core.Interfaces;
using BookHub.Core.Services;
using BookHub.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Core
{
    public static class DependenciesConfiguration
    {
        /// <summary>
        /// It manages of life cycle of services
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <returns>services</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterInfrastructureDependencies(configuration);

            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IBookService, BookService>();

            return services;
        }
    }
}
