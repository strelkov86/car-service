using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IRepairTypesService
    {
        public Task<IEnumerable<RepairTypeDTO>> GetAll();
        public Task<RepairTypeDTO> GetById(int id);
        public Task<RepairTypeDTO> Create(RepairTypeDTO dto);
        public Task<RepairTypeDTO> Update(RepairTypeDTO dto);
        public Task<int> Delete(int id);
    }
}
