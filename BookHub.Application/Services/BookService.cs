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
        private readonly IAuthorService _authorService;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, IAuthorService authorService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authorService = authorService;
        }

        public async Task<BookModel> GetBookById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id is not valid", nameof(id));
            }

            var entity = await _unitOfWork.Books.GetByIdAsync(id, includeProperties: "Author")
                         ?? throw new InvalidOperationException("Book is null");

            var model = _mapper.Map<BookModel>(entity);
            return model;
        }

        public async Task<IReadOnlyCollection<BookModel>> GetAllBooks()
        {
            var entities = await _unitOfWork.Books.GetAllAsync(includeProperties: "Author")
                ?? throw new InvalidOperationException("Books collection is null");

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
