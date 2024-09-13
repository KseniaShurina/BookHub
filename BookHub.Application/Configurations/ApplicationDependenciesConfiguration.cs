using BookHub.Application.Interfaces;
using BookHub.Application.Services;
using BookHub.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Application.Configurations
{
    public static class ApplicationDependenciesConfiguration
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
            services.AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}
