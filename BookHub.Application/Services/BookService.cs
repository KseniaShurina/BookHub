using AutoMapper;
using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;

namespace BookHub.Application.Services
{
    internal class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookModel> GetBookById(Guid id)
        {
            var entity = await _unitOfWork.Books.GetByIdAsync(id)
                         ?? throw new NullReferenceException("Book is null");
            var model = _mapper.Map<BookModel>(entity);
            return model;
        }

        public async Task<IReadOnlyCollection<BookModel>> GetAllBooks()
        {
            var entities = await _unitOfWork.Books.GetAllAsync();

            var models = _mapper.Map<List<BookModel>>(entities);

            return models;
        }

        public async Task<Guid> CreateBook(CreateBookModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "The book entity cannot be null.");
            }

            var entity = new Book
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                AuthorId = model.AuthorId,
            };

            await _unitOfWork.Books.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return entity.Id;
        }
    }
}
