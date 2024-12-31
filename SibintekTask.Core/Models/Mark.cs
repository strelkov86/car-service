using System.Collections.Generic;

namespace SibintekTask.Core.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public IEnumerable<Vehicle>? Vehicles { get; set; }
    }
}
