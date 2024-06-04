using AutoMapper;
using BookHub.Application.Models;
using BookHub.Core.Entities;

namespace BookHub.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookModel>();
            CreateMap<BookModel, Book>();
        }
    }
}
