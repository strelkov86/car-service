using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<User>?> GetAll(CancellationToken token = default);
        public Task<User?> GetById(int id, CancellationToken token = default);
        public Task<User?> GetByITN(string itn, CancellationToken token = default);
        public Task<IEnumerable<User>?> GetByRole(int roleId, CancellationToken token = default);
        public Task<User> Create(User user, CancellationToken token = default);
        public Task<User?> Update(User user, CancellationToken token = default);
        public Task<int> Delete(int id, CancellationToken token = default);
        public Task<bool> AttachRoleToUser(int userId, int roleId, CancellationToken token = default);
        public Task<bool> RemoveRoleFromUser (int userId, int roleId, CancellationToken token = default);
        public Task<User> SetPasswordHash(int userId, string passwordHash, CancellationToken token = default);
    }
}
