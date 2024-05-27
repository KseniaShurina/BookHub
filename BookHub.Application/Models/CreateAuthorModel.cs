using BookHub.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookHub.Application.Models
{
    internal class CreateAuthorModel
    {
        public Guid Id { get; init; }

        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfDeath { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = null!;
        public List<Book> Books { get; set; } = null!;
    }
}
