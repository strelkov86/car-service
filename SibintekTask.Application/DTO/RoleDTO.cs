using System.Collections.Generic;

namespace SibintekTask.Application.DTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleDetails : RoleDTO
    {
        public ICollection<UserDTO> Users { get; set; }
    }
}
