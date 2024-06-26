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

        public async Task<bool> IsAuthorExistAsync(string firstName, string lastName)
        {
            return await _unitOfWork.Authors.ExistsAsync(
                a => a.FirstName == firstName && a.LastName == lastName
            );
        }

        public async Task<AuthorModel> GetAuthorById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id is not valid", nameof(id));
            }

            var entity = await _unitOfWork.Authors.GetByIdAsync(id, includeProperties: "Books") 
                         ?? throw new InvalidOperationException("Author is null");

            var model = _mapper.Map<AuthorModel>(entity);
            return model;
        }

        public async Task<IReadOnlyCollection<AuthorModel>> GetAllAuthors()
        {
            var entities = await _unitOfWork.Authors.GetAllAsync(includeProperties: "Books")
                           ?? throw new InvalidOperationException("Authors collection is null");

            var models = _mapper.Map<List<AuthorModel>>(entities);

            return models;
        }

        public async Task<Guid> CreateAuthor(CreateAuthorModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The author entity cannot be null.");
            }

            bool exists = await IsAuthorExistAsync(model.FirstName, model.LastName);

            if (exists)
            {
                throw new InvalidOperationException("Author with the same name already exists.");
            }

            var entity = new Author
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                DateOfDeath = model.DateOfDeath,
                Description = model.Description
            };

            await _unitOfWork.Authors.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}
