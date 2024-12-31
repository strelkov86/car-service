using System;

namespace SibintekTask.Application.DTO
{
    public class RepairDTO
    {

        public int Id { get; set; }
        public int RepairTypeId { get; set; }
        public RepairTypeDTO? RepairType { get; set; }
        public int VehicleId { get; set; }
        public VehicleDTO? Vehicle { get; set; }
        public DateTime AcceptedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int Cost { get; set; }
        public int Mileage { get; set; }
        public int CustomerId { get; set; }
        public UserDTO? Customer { get; set; }
        public int ExecutorId { get; set; }
        public UserDTO? Executor { get; set; }
    }
}
