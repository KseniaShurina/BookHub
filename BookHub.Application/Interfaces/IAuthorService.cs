using BookHub.Application.Models;

namespace BookHub.Application.Interfaces;

public interface IAuthorService
{
    Task<AuthorModel> GetAuthorById(Guid id);
    Task<IReadOnlyCollection<AuthorModel>> GetAllAuthors();
    Task<Guid> CreateAuthor(CreateAuthorModel entity);
    Task DeleteAuthor(Guid id);
}