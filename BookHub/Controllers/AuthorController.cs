using AutoMapper;
using BookHub.Application.Interfaces;
using BookHub.Application.Models;
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
        private readonly IMapper _mapper;
        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService, IMapper mapper)
        {
            _logger = logger;
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<AuthorModel> Get(Guid id)
        {
            var entity = await _authorService.GetAuthorById(id);
            var result = _mapper.Map<AuthorModel>(entity);
            return result;
        }

        [HttpPost]
        public async Task<Author> Add(CreateAuthorModel author)
        {
            var id = await _authorService.CreateAuthor(author);
            var response = await _authorService.GetAuthorById(id);
            return response;
        }
    }
}
