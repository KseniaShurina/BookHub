using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookService _bookService;

        public BookController(ILogger<BookController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public async Task<BookModel> Get(Guid id)
        {
            var result = await _bookService.GetBookById(id);

            return result;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<BookModel>> GetAll()
        {
            return await _bookService.GetAllBooks();
        }

        [HttpPost]
        public async Task<Guid> Add(CreateBookModel book)
        {
            var id = await _bookService.CreateBook(book);
            return id;
        }
    }
}
