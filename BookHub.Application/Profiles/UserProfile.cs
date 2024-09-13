using AutoMapper;
using BookHub.Application.Models;
using BookHub.Core.Entities;

namespace BookHub.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
            CreateMap<UserRegisterModel, User>();
        }
    }
}
