using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;

namespace BookHub.Application.Services
{
    internal class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Author> GetAuthorById(Guid id)
        {
            var result = await _unitOfWork.Authors.GetByIdAsync(id);

            if (result == null)
            {
                throw new NullReferenceException(nameof(result));
            }
            else
            {
                return result;
            }
        }

    //    public Task<Author> CreateAuthor(Author entity)
    //    {
    //        var author = new Author
    //        {
    //            Id = Guid.NewGuid(),
    //            FirstName = entity.FirstName,
    //            LastName = entity.LastName,
    //            DateOfBirth = entity.DateOfBirth,
    //            DateOfDeath = entity.DateOfDeath,
    //            Description = entity.Description
    //        };
    //        throw new NotImplementedException();
    //        _context.Authors.Add(author);
    //        await _context.SaveChangesAsync(author);

    //        return CreateAuthorModel;
    //    }
    }
}
