using AutoMapper;
using BookHub.Application.Models;
using BookHub.Core.Entities;

namespace BookHub.Application.Profiles
{
    internal class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorModel>();
            CreateMap<AuthorModel, Author>();
        }
    }
}
