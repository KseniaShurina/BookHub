namespace BookHub.Application.Models
{
    public class UserModel
    {
        public Guid Id { get; init; }
        public string Email { get; set; } = null!;
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
    }
}
