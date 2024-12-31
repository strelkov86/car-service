using System;

namespace SibintekTask.Core.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public int RepairTypeId { get; set; }
        public RepairType? RepairType { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime AcceptedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int Cost { get; set; }
        public int Mileage { get; set; }
        public int CustomerId { get; set; }
        public User? Customer { get; set; }
        public int ExecutorId { get; set; }
        public User? Executor { get; set; }
    }
}
