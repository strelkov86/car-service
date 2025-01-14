using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IMarksRepository
    {
        public Task<IEnumerable<Mark>> GetAll(CancellationToken token = default);
        public Task<Mark> GetById(int id, CancellationToken token = default);
        public Task<Mark> GetByName(string name, CancellationToken token = default);
        public Task<Mark> Create(Mark mark, CancellationToken token = default);
        public Task<Mark> Update(Mark mark, CancellationToken token = default);
        public Task<int> Delete (int id, CancellationToken token = default);
    }
}
