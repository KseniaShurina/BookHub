namespace BookHub.Application.Models
{
    public class AuthorModelInfo
    {
        public Guid Id { get; init; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfDeath { get; set; }
    }
}
