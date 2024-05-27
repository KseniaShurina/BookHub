using BookHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
