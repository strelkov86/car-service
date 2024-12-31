using System.Collections.Generic;

namespace SibintekTask.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? ITN { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PasswordHash { get; set; }
        public ICollection<Role>? Roles { get; set; }

        public IEnumerable<Repair>? CustomerRepairs { get; set; }
        public IEnumerable<Repair>? ExecutorRepairs { get; set; }
    }
}
