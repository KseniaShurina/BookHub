using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Application.Configurations
{
    internal class AutoMapperConfiguration
    {
        public AutoMapperConfiguration() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services">Represents the collection of services to which we add AutoMapper</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IServiceCollection AddAutoMapperProfiles(IServiceCollection services)
        {
            var configuration = new AutoMapperConfiguration();
            throw new NotImplementedException();
        }
    }
}
