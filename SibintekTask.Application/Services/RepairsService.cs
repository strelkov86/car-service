using AutoMapper;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class RepairsService : IRepairsService
    {
        private readonly IRepairsRepository _repairs;
        private readonly IMapper _mapper;
        public RepairsService(IRepairsRepository r, IMapper m)
        {
            _repairs = r;
            _mapper = m;
        }

        public async Task<RepairDTO> Create(RepairDTO dto)
        {
            var model = _mapper.Map<Repair>(dto);
            model.AcceptedAt = DateTime.Now;
            var repair = await _repairs.Create(model);
            return _mapper.Map<RepairDTO>(repair);
        }

        public async Task<int> Delete(int id)
        {
            return await _repairs.Delete(id);
        }

        public async Task<IEnumerable<RepairDTO>?> GetAll()
        {
            var repairs = await _repairs.GetAll();
            if (!repairs.Any()) return null;
            return _mapper.Map<IEnumerable<RepairDTO>?>(repairs);
        }

        public async Task<RepairDTO?> GetById(int id)
        {
            var repair = await _repairs.GetById(id);
            return _mapper.Map<RepairDTO>(repair);
        }

        public async Task<RepairDTO?> Update(RepairDTO dto)
        {
            var model = _mapper.Map<Repair>(dto);
            var repair = await _repairs.Update(model);
            return _mapper.Map<RepairDTO?>(repair);
        }

        public async Task<RepairDTO?> FinishRepair(int id)
        {
            var repair = await _repairs.FinishRepair(id);
            return _mapper.Map<RepairDTO>(repair);
        }
    }
}
