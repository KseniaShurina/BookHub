namespace BookHub.Core.Entities
{
    public class User
    {
        public Guid Id { get; init; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public string? FirstName { get; set; } = null!;
        public string? LastName { get; set; } = null!;
    }
}
