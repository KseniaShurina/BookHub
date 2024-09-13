using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace BookHub.Application.Services
{
    internal class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly PasswordHasher<User> _passwordHasher;
        private readonly string _jwtSecret;
        private readonly string? _jwtIssuer;
        private readonly string _jwtAudience;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
            _jwtSecret = configuration["Jwt:SecretKey"] ?? throw new ArgumentNullException(nameof(_jwtSecret));
            _jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(_jwtIssuer));
            _jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(_jwtAudience));
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            var user = await _unitOfWork.Users.FindByConditionAsync(u => u.Email == email);

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException(nameof(password));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token).ToString();
        }

        public async Task RegisterUserAsync(UserRegisterModel model)
        {
            var exists = await _unitOfWork.Users.ExistsAsync(u => u.Email == model.Email);
            if (exists) throw new ArgumentException("User with these parameters already exist", $"{nameof(model.Email)}");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
