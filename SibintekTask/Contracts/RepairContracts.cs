using System;
using System.ComponentModel.DataAnnotations;

namespace SibintekTask.API.Contracts
{
    public record CreateRepairRequest([Required] int RepairTypeId, [Required] int Cost, [Required] int CustomerId, [Required] int ExecutorId, [Required] int VehicleId, [Required] int Mileage);
    public record UpdateRepairRequest(int RepairTypeId, int Cost, DateTime AcceptedAt, int CustomerId, int ExecutorId, int VehicleId, int Mileage);
}
