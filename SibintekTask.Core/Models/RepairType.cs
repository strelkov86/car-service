using System.Collections.Generic;

namespace SibintekTask.Core.Models
{
    public class RepairType
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public IEnumerable<Repair>? Repairs { get; set; }
    }
}