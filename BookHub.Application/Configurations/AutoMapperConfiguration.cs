using AutoMapper;
using BookHub.Application.Models;
using BookHub.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Application.Configurations
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection AddAutoMapperConfigurations(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, CreateAuthorModel>();
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
