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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.FindByConditionAsync(u => u.Email == email) ?? null;

            return user;
        }

        public async Task<bool> IsExistsInExternalLoginProviderUserAsync(string email)
        {
            var isExists = await _unitOfWork.UserExternalLoginProviders.ExistsAsync(i => i.User.Email == email);
            return isExists;
        }

        public async Task<bool> IsExistsUserAsync(string email)
        {
            var isExists = await _unitOfWork.Users.ExistsAsync(u => u.Email == email);
            return isExists;
        }

        public async Task<string> GenerateTokenAsync(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user is null)
            {
                throw new NullReferenceException($"{nameof(user)} is null");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> CheckPassword(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);

            if (user is null)
            {
                throw new NullReferenceException($"{nameof(user)} is null");
            }

            return _passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, password) == PasswordVerificationResult.Success;
        }

        public async Task RegisterUserAsync(UserRegisterModel model)
        {
            var isExists = await IsExistsUserAsync(model.Email);

            if (isExists) throw new ArgumentException("User with this email already exists");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RegisterUserByExternalProviderAsync(string email, string scheme, string externalId)
        {
            var newUser = new UserRegisterModel
            {
                Email = email,
                Password = externalId + "!",
            };
            await RegisterUserAsync(newUser);


            var user = await GetUserByEmailAsync(email);

            if (user is null)
            {
                throw new NullReferenceException($"{nameof(user)} is null");
            }

            var model = new UserExternalProviderRegisterModel
            {
                Email = email,
                Scheme = scheme,
                ExternalId = externalId,
                UserId = user.Id,
            };

            var provider = _mapper.Map<UserExternalLoginProvider>(model);

            await _unitOfWork.UserExternalLoginProviders.AddAsync(provider);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}