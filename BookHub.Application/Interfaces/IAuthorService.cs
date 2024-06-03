using BookHub.Application.Models;
using BookHub.Core.Entities;

namespace BookHub.Application.Interfaces;

public interface IAuthorService
{
    Task<Author> GetAuthorById(Guid id);
    Task<Guid> CreateAuthor(CreateAuthorModel entity);
}