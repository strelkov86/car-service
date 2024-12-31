using AutoMapper;
using System.Threading.Tasks;
using SibintekTask.Core.Models;
using SibintekTask.Core.Interfaces;
using SibintekTask.Application.DTO;
using System.Linq;

namespace SibintekTask.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _users;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        public AuthService(IUsersRepository u, IJwtService j, IMapper m)
        {
             _users = u;
            _jwtService = j;
            _mapper = m;
        }

        public async Task<string> Login(string itn, string password)
        {
            var exists = await _users.GetByITN(itn);
            if (exists is null) return null;

            var passwordMatched = PasswordHasher.Verify(password, exists.PasswordHash);
            if (!passwordMatched) return null;

            var token = _jwtService.GenerateToken(exists);
            return token;
        }

        public async Task<UserDTO> Register(string name, string surname, string itn, string password)
        {
            var exists = await _users.GetByITN(itn);
            if (exists != null) return null;

            var hashedPassword = PasswordHasher.GenerateHash(password);
            var model = new User() { Name = name, Surname = surname, ITN = itn, PasswordHash = hashedPassword };
            var user = await _users.Create(model);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> RegisterForExistingCustomer(string itn, string password)
        {
            var exists = await _users.GetByITN(itn);
            if (exists is null) return null;

            var hashedPassword = PasswordHasher.GenerateHash(password);
            var user = await _users.SetPasswordHash(exists.Id, hashedPassword);

            return _mapper.Map<UserDTO>(user);
        }
    }
}
