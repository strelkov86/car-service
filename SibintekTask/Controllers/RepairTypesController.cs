using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("repair-types")]
    [Authorize(Roles = "Manager, Executor")]
    public class RepairTypesController : ControllerBase
    {
        private readonly IRepairTypesService _typesService;
        public RepairTypesController(IRepairTypesService s)
        {
            _typesService = s;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRepairTypes()
        {
            var types = await _typesService.GetAll();
            if (types is null) return NoContent();
            return Ok(new { Types = types });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRepairTypeById(int id)
        {
            var type = await _typesService.GetById(id);
            return Ok(new { Type = type });
        }

        [HttpPost]
        public async Task<ActionResult> CreateRepairType([FromBody] SimpleEntityContract request)
        {
            var typeDto = new RepairTypeDTO() { Name = request.Name };
            var created = await _typesService.Create(typeDto);
            return Ok(new { Created = created });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRepairType(int id, [FromBody] SimpleEntityContract request)
        {
            var typeDto = new RepairTypeDTO() { Id = id, Name = request.Name };
            var updated = await _typesService.Update(typeDto);
            return Ok(new { Updated = updated });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteRepairType(int id)
        {
            var deleted = await _typesService.Delete(id);
            return Ok(new { DeletedCount = deleted });
        }
    }
}
