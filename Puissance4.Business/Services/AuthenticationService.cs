using FluentValidation;
using FluentValidation.Results;
using Puissance4.Business.DTO;
using Puissance4.Business.Interfaces;
using Puissance4.Domain.Entities;
using System.Net.Http.Headers;
using System.Security.Authentication;

namespace Puissance4.Business.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHashService _hashService;

        public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, IHashService hashService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _hashService = hashService;
        }

        public async Task<AuthenticationDTO> LoginAsync(LoginDTO dto)
        {
            User? user = await _userRepository.FindByUsernameAsync(dto.Username);
            if (user == null || !_hashService.VerifyPassword(user.Password, dto.Password, user.Salt))
            {
                throw new AuthenticationException();
            }
            return new AuthenticationDTO
            {
                Token = _jwtService.CreateToken(user.Id.ToString(), user.Username),
                Id = user.Id,
                Username = user.Username,
            };
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            if(await _userRepository.AnyByUsernameAsync(dto.Username))
            {
                throw new ValidationException("Bad Request", new List<ValidationFailure>
                {
                    new ValidationFailure("Username", "Username already exists")
                });
            }
            Guid salt = Guid.NewGuid();
            byte[] hashed = _hashService.HashPassword(dto.Password, salt);
            await _userRepository.AddAsync(new User
            {
                Username = dto.Username,
                Password = hashed,
                Salt = salt,
            });
        }
    }
}
