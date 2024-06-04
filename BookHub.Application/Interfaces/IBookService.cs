using BookHub.Application.Models;

namespace BookHub.Application.Interfaces;

public interface IBookService
{
    Task<BookModel> GetBookById(Guid id);
    Task<IReadOnlyCollection<BookModel>> GetAllBooks();
    Task<Guid> CreateBook(CreateBookModel model);
}