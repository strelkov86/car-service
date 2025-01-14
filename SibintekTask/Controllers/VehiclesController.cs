using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SibintekTask.API.Helpers;
using SibintekTask.API.Contracts;
using SibintekTask.Application.DTO;
using SibintekTask.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SibintekTask.API.Controllers
{
    [ApiController]
    [Route("vehicles")]
    [Authorize(Roles = "Manager")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService _vehiclesService;
        public VehiclesController(IVehiclesService v)
        {
            _vehiclesService = v;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllVehicles()
        {
            var vehicles = await _vehiclesService.GetAll();
            if (vehicles is null) return NoContent();
            var response = VehiclesHelper.PrepareVehicleListResponse(vehicles);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetVehicleById(int id)
        {
            var vehicle = await _vehiclesService.GetById(id);
            var response = VehiclesHelper.PrepareVehicleResponse(vehicle);
            return Ok(new { Vehicle = response });
        }

        [HttpPost]
        public async Task<ActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            var vehicleDto = new VehicleDTO() { NumberPlate = request.NumberPlate, MarkId = request.MarkId };
            var created = await _vehiclesService.Create(vehicleDto);
            var response = VehiclesHelper.PrepareVehicleResponse(created);
            return CreatedAtAction(nameof(GetVehicleById), new { id = created.Id }, new { Created = response });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleRequest request)
        {
            var vehicleDto = new VehicleDTO() { Id = id, NumberPlate = request.NumberPlate, MarkId = request.MarkId };
            var updated = await _vehiclesService.Update(vehicleDto);
            var response = VehiclesHelper.PrepareVehicleResponse(updated);
            return Ok(new { Updated = response });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var deleted = await _vehiclesService.Delete(id);
            return Ok(new { DeletedCount = deleted });
        }
    }
}
