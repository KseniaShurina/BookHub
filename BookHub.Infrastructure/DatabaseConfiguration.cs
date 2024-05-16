using BookHub.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure
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
            services.AddDbContext<IApplicationContext, ApplicationContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("Database:ConnectionString");
                var isLogEnabled = configuration.GetValue<bool>("Database:IsLogEnabled");
                options.UseNpgsql(connectionString);
                if (isLogEnabled)
                {
                    options.LogTo(Console.WriteLine);
                }
            }, ServiceLifetime.Transient);
        }
    }
}
