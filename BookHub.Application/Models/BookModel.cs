using System.ComponentModel.DataAnnotations;

namespace BookHub.Application.Models
{
    public class BookModel
    {
        public Guid Id { get; init; }

        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(1000)]
        public string Description { get; set; } = null!;

        public AuthorModelInfo Author { get; set; } = null!;
    }
}
