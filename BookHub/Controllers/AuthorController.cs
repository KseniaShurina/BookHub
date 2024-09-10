using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorService _authorService;

        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService)
        {
            _logger = logger;
            _authorService = authorService;
        }

        [Route("{id:guid}")]
        [HttpGet]
        public async Task<AuthorModel> Get(Guid id)
        {
            var result = await _authorService.GetAuthorById(id);
            return result;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<AuthorModel>> GetAll()
        {
            return await _authorService.GetAllAuthors();
        }

        [HttpPost]
        public async Task<string> Add(CreateAuthorModel author)
        {
            var id = await _authorService.CreateAuthor(author);
            return $"Author was created. ID: {id}";
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete(Guid id)
        {
            await _authorService.DeleteAuthor(id);
            return $"Author {id} is deleted";
        }
    }
}
