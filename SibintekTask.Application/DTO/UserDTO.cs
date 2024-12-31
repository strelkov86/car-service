using SibintekTask.Core.Models;
using System.Collections.Generic;

namespace SibintekTask.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? ITN { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public ICollection<RoleDTO>? Roles { get; set; }
    }

    public class UserDetails : UserDTO
    {
        public ICollection<RepairDTO> Repairs { get; set; }
    }
}
