using BookHub.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHub.Infrastructure.Configurations
{
    /// <summary>
    /// Provides methods for configuring database-related services and dependencies.
    /// </summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Registers infrastructure dependencies
        /// </summary>
        /// <param name="services">The service collection to add dependencies to.</param>
        /// <param name="configuration">The application configuration containing connection string and other settings.</param>
        /// <exception cref="ArgumentException">Thrown when the connection string is not configured.</exception>
        public static void RegisterInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //Reading and checking connection string from configuration
            var connectionString = configuration.GetValue<string>("Database:ConnectionString")
                                   ?? throw new ArgumentException("Database:ConnectionString is not configured.");

            //Reading and checking the logging flag from the configuration
            var isLogEnabled = configuration.GetValue<bool>("Database:IsLogEnabled");

            //Registering a Database Context
            services.AddDbContext<IApplicationContext, ApplicationContext>(options =>
            {
                options.UseNpgsql(connectionString);
                if (isLogEnabled)
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information);
                }
            }, ServiceLifetime.Transient); // Transient lifecycle for DbContext

            // Registering the Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
