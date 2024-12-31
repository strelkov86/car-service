using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IRolesRepository
    {
        public Task<IEnumerable<Role>> GetAll(CancellationToken token = default);
        public Task<Role?> GetById(int id, CancellationToken token = default);
        public Task<Role?> GetByName(string name, CancellationToken token = default);
        public Task<Role> Create(Role role, CancellationToken token = default);
        public Task<Role?> Update(Role role, CancellationToken token = default);
        public Task<int> Delete(int id, CancellationToken token = default);
    }
}
