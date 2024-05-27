using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BookHub.Application.Services
{
    internal class AuthorService : IAuthorService
    {
        private readonly IApplicationContext _context;

        public AuthorService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthorById(Guid id)
        {
            var result = await _context.Authors.FirstOrDefaultAsync(i => i.Id == id);

            if (result is null)
            {
                throw new NullReferenceException(nameof(result));
            }
            else
            {
                return result;
            }
        }

        public async Task<CreateAuthorModel> CreateAuthor(Author entity)
        {
            //TODO Add Validation

            var author = new CreateAuthorModel
            {
                Id = Guid.NewGuid(),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth,
                DateOfDeath = entity.DateOfDeath,
                Description = entity.Description
            };
            throw new NotImplementedException();
            //_context.Authors.Add(author);
            //await _context.SaveChangesAsync(author);

            //return CreateAuthorModel;
        }
    }
}
