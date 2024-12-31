using SibintekTask.Core.Models;
using System.Collections.Generic;

namespace SibintekTask.Application.DTO
{
    public class VehicleDTO
    {
        public int Id { get; set; }
        public string? NumberPlate { get; set; }
        public int? MarkId { get; set; }
        public MarkDTO? Mark { get; set; }
    }

    public class VehicleDetails : VehicleDTO
    {
        public IEnumerable<RepairDTO>? Repairs { get; set; }
    }
        
}
