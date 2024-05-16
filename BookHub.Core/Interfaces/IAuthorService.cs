using BookHub.Domain.Entities;

namespace BookHub.Core.Interfaces;

public interface IAuthorService
{
    Task<Author> GetAuthorById(Guid id);
}