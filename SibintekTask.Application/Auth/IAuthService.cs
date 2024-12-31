using SibintekTask.Application.DTO;
using System.Threading.Tasks;

namespace SibintekTask.Application.Auth
{
    public interface IAuthService
    {
        public Task<UserDTO> Register(string name, string surname, string itn, string password);
        public Task<string> Login(string itn, string password);
        public Task<UserDTO> RegisterForExistingCustomer(string itn, string password);
    }
}
