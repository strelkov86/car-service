using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IMarksService
    {
        public Task<IEnumerable<MarkDTO>> GetAll();
        public Task<MarkDTO?> GetById(int id);
        public Task<MarkDTO> Create(MarkDTO dto);
        public Task<MarkDTO?> Update(MarkDTO dto);
        public Task<int> Delete(int id);
    }
}
