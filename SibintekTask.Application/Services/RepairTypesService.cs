using AutoMapper;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class RepairTypesService : IRepairTypesService
    {
        private readonly IRepairTypesRepository _types;
        private readonly IMapper _mapper;
        public RepairTypesService(IRepairTypesRepository r, IMapper m)
        {
            _types = r;
            _mapper = m;
        }

        public async Task<RepairTypeDTO> Create(RepairTypeDTO dto)
        {
            var exists = await _types.GetByName(dto.Name);
            if (exists is not null) return _mapper.Map<RepairTypeDTO>(exists);

            var model = _mapper.Map<RepairType>(dto);
            var type = await _types.Create(model);
            return _mapper.Map<RepairTypeDTO>(type);
        }

        public async Task<int> Delete(int id)
        {
            return await _types.Delete(id);
        }

        public async Task<IEnumerable<RepairTypeDTO>> GetAll()
        {
            var types = await _types.GetAll();
            if (!types.Any()) return null;
            return _mapper.Map<IEnumerable<RepairTypeDTO>>(types);
        }

        public async Task<RepairTypeDTO?> GetById(int id)
        {
            var type = await _types.GetById(id);
            return _mapper.Map<RepairTypeDTO?>(type);
        }

        public async Task<RepairTypeDTO?> Update(RepairTypeDTO dto)
        {
            var model = _mapper.Map<RepairType>(dto);
            var type = await _types.Update(model);
            return _mapper.Map<RepairTypeDTO?>(type);
        }
    }
}
