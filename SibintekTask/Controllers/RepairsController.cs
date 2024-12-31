using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Contracts;
using SibintekTask.API.Helpers;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using System.Threading.Tasks;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("repairs")]
    [Authorize(Roles = "Manager, Executor")]
    public class RepairsController : ControllerBase
    {
        private readonly IRepairsService _repairsService;
        public RepairsController(IRepairsService s)
        {
            _repairsService = s;
        }

        [HttpGet]
        public async Task<ActionResult<RepairDTO>> GetRepairs()
        {
            var repairs = await _repairsService.GetAll();
            if (repairs is null) return NoContent();
            var response = RepairsHelper.PrepareRepairListResponse(repairs);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RepairDTO>> GetRepairById(int id)
        {
            var repair = await _repairsService.GetById(id);
            if (repair is null) return NotFound($"Ремонт {id} не найден");
            var response = RepairsHelper.PrepareRepairResponse(repair);
            return Ok(new { Repair = response });
        }

        [HttpPost]
        public async Task<ActionResult<RepairDTO>> CreateRepair([FromBody] CreateRepairRequest request)
        {
            var dto = new RepairDTO()
            {
                RepairTypeId = request.RepairTypeId,
                Cost = request.Cost,
                VehicleId = request.VehicleId,
                Mileage = request.Mileage,
                CustomerId = request.CustomerId,
                ExecutorId = request.ExecutorId
            };
            var created = await _repairsService.Create(dto);
            var response = RepairsHelper.PrepareRepairResponse(created);
            return CreatedAtAction(nameof(GetRepairById), new { id = created.Id }, new { Created = response });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RepairDTO>> UpdateRepair(int id, [FromBody] UpdateRepairRequest request)
        {
            var dto = new RepairDTO()
            {
                RepairTypeId = request.RepairTypeId,
                Cost = request.Cost,
                AcceptedAt = request.AcceptedAt,
                VehicleId = request.VehicleId,
                Mileage = request.Mileage,
                CustomerId = request.CustomerId,
                ExecutorId = request.ExecutorId
            };
            var updated = await _repairsService.Update(dto);
            if (updated is null) return NotFound($"Ремонт {id} не найден");
            var response = RepairsHelper.PrepareRepairResponse(updated);
            return Ok(new { Updated = response });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<int>> DeleteRepair(int id)
        {
            var deleted = await _repairsService.Delete(id);
            if (deleted == 0) return NotFound($"Ремонт {id} не найден");
            return Ok(new { DeletedCount = deleted });
        }

        [HttpPatch("{id}/issue")]
        public async Task<ActionResult> IssueRepair(int id)
        {
            var repair = await _repairsService.FinishRepair(id);
            if (repair is null) return NotFound($"Ремонт {id} не найден");
            return Ok($"{repair.RepairType} для машины {repair.Vehicle.Mark} {repair.Vehicle.NumberPlate} завершен. Заказчик {repair.Customer} принял машину {repair.FinishedAt} от исполнителя {repair.Executor}");
        }
    }
}
