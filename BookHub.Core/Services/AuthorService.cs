using BookHub.Core.Interfaces;
using BookHub.Domain.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Core.Services
{
    internal class AuthorService : IAuthorService
    {
        private readonly IApplicationContext _context;

        public AuthorService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthorById(Guid id)
        {
            var result = await _context.Authors.FirstOrDefaultAsync(i => i.Id == id);

            if (result is null)
            {
                throw new NullReferenceException(nameof(result));
            }
            else
            {
                return result;
            }
        }
    }
}
