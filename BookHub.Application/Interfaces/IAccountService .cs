using BookHub.Application.Models;

namespace BookHub.Application.Interfaces
{
    public interface IAccountService
    {
        Task<string> AuthenticateUserAsync(string email, string password);
        Task RegisterUserAsync(UserRegisterModel model);
    }
}
