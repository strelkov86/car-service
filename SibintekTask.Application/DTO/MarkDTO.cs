using System.Collections.Generic;

namespace SibintekTask.Application.DTO
{
    public class MarkDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MarkDetails : MarkDTO
    {
        public ICollection<VehicleDTO> Vehicles { get; set; }
    }
}
