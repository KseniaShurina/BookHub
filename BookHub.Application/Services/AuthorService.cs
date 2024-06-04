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
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthorModel> GetAuthorById(Guid id)
        {
            var entity = await _unitOfWork.Authors.GetByIdAsync(id) 
                         ?? throw new NullReferenceException("Author is null");
            var model = _mapper.Map<AuthorModel>(entity);
            return model;
        }

        public async Task<IReadOnlyCollection<AuthorModel>> GetAllAuthors()
        {
            var entities = await _unitOfWork.Authors.GetAllAsync();

            var models = _mapper.Map<List<AuthorModel>>(entities);

            return models;
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
