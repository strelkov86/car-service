using System.Collections.Generic;

namespace SibintekTask.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? NumberPlate { get; set; }
        public int? MarkId { get; set; }
        public Mark? Mark { get; set; }

        public IEnumerable<Repair>? Repairs { get; set; }
    }
}
