using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IRepairTypesRepository
    {
        public Task<IEnumerable<RepairType>> GetAll(CancellationToken token = default);
        public Task<RepairType> GetById(int id, CancellationToken token = default);
        public Task<RepairType> GetByName(string name, CancellationToken token = default);
        public Task<RepairType> Create(RepairType type, CancellationToken token = default);
        public Task<RepairType> Update(RepairType type, CancellationToken token = default);
        public Task<int> Delete(int id, CancellationToken token = default);
    }
}
