using AutoMapper;
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
            var entity = await _unitOfWork.Authors.GetByIdAsync(id) 
                         ?? throw new NullReferenceException("Author is null");
            return entity;
        }

        public async Task<Guid> CreateAuthor(CreateAuthorModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The author entity cannot be null.");
            }

            var author = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth,
                DateOfDeath = entity.DateOfDeath,
                Description = entity.Description
            };

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();

            return author.Id;
        }
    }
}
