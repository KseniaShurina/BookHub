using AutoMapper;
using BookHub.Application.Models;
using BookHub.Core.Entities;

namespace BookHub.Application.Profiles
{
    public class UserExternalLoginProviderProfile : Profile
    {
        public UserExternalLoginProviderProfile()
        {
            CreateMap<UserExternalProviderRegisterModel, UserExternalLoginProvider>();
            CreateMap<UserExternalLoginProvider, UserExternalProviderRegisterModel>();
        }
    }
}
