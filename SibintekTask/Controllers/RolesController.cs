using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("roles")]
    [Authorize(Roles = "Manager, Executor")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        public RolesController(IRolesService s)
        {
            _rolesService = s;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _rolesService.GetAll();
            if (roles is null) return NoContent();
            return Ok(new { Roles = roles});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoleById(int id)
        {
            var role = await _rolesService.GetById(id);
            return Ok(new { Role = role });
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole([FromBody] SimpleEntityContract request)
        {
            var roleDto = new RoleDTO() { Name = request.Name };
            var created = await _rolesService.Create(roleDto);
            return Ok(new { Created = created });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, [FromBody] SimpleEntityContract request)
        {
            var roleDto = new RoleDTO() { Id = id, Name = request.Name };
            var updated = await _rolesService.Update(roleDto);
            return Ok(new { Updated = updated });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var deleted = await _rolesService.Delete(id);
            return Ok(new { DeletedCount = deleted });
        }
    }
}
