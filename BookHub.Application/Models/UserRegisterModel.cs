using System.ComponentModel.DataAnnotations;

namespace BookHub.Application.Models
{
    public class UserRegisterModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required] 
        public string Password { get; set; } = null!;
    }
}
