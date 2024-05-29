using BookHub.Application.Interfaces;
using BookHub.Core.Entities;
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
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public Task<Author> Get(Guid id)
        {
            var response = _authorService.GetAuthorById(id);
            return response;
        }
    }
}
