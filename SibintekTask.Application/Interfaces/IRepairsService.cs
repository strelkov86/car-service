using SibintekTask.Application.DTO;
using SibintekTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IRepairsService
    {
        public Task<IEnumerable<RepairDTO>?> GetAll();
        public Task<RepairDTO?> GetById(int id);
        public Task<RepairDTO> Create(RepairDTO dto);
        public Task<RepairDTO?> Update(RepairDTO dto);
        public Task<int> Delete(int id);

        public Task<RepairDTO?> FinishRepair(int id);
    }
}
