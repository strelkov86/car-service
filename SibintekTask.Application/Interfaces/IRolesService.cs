using SibintekTask.Application.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SibintekTask.Application.Interfaces
{
    public interface IRolesService
    {
        public Task<IEnumerable<RoleDTO>> GetAll();
        public Task<RoleDTO?> GetById(int id);
        public Task<RoleDTO> Create(RoleDTO dto);
        public Task<RoleDTO?> Update(RoleDTO dto);
        public Task<int> Delete(int id);
    }
}
