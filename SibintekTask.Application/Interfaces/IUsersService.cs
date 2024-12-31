using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IUsersService
    {
        public Task<IEnumerable<UserDTO>?> GetAll();
        public Task<UserDTO?> GetById(int id);
        public Task<UserDTO?> GetByITN(string itn);
        public Task<IEnumerable<UserDTO>?> GetByRole(int roleId);
        public Task<UserDTO> CreateCustomer(UserDTO userDto);
        public Task<UserDTO?> Update(UserDTO userDto);
        public Task<int> Delete(int id);
        public Task<bool> GrantRoleToUser(int userId, int roleId);
        public Task<bool> RevokeRoleFromUser(int userId, int roleId);
    }
}
