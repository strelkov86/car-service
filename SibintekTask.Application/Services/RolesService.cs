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
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository _roles;
        private readonly IMapper _mapper;
        public RolesService(IRolesRepository r, IMapper m)
        {
            _roles = r;
            _mapper = m;
        }

        public async Task<RoleDTO> Create(RoleDTO dto)
        {
            var exists = await _roles.GetByName(dto.Name);
            if (exists is not null) return _mapper.Map<RoleDTO>(exists);

            var model = _mapper.Map<Role>(dto);
            var role = await _roles.Create(model);
            return _mapper.Map<RoleDTO>(role);
        }

        public async Task<int> Delete(int id)
        {
            return await _roles.Delete(id);
        }

        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await _roles.GetAll();
            if (!roles.Any()) return null;
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task<RoleDTO?> GetById(int id)
        {
            var role = await _roles.GetById(id);
            return _mapper.Map<RoleDTO?>(role);
        }

        public async Task<RoleDTO?> Update(RoleDTO dto)
        {
            var model = _mapper.Map<Role>(dto);
            var role = await _roles.Update(model);
            return _mapper.Map<RoleDTO?>(role);
        }
    }
}
