using System.Collections.Generic;

namespace SibintekTask.Application.DTO
{
    public class RepairTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RepairTypeDetails : RepairTypeDTO
    {
        public ICollection<RepairDTO> Repairs { get; set; }
    }
}
