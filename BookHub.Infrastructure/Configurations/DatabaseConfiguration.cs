using BookHub.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookHub.Infrastructure.Configurations
{
    public static class DatabaseConfiguration
    {
        /// <summary>
        /// Registers infrastructure dependencies, such as database context,
        /// using Entity Framework Core.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
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
                                          
            //Register UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
