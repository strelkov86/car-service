using Microsoft.EntityFrameworkCore;
using SibintekTask.Core.Exceptions;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Persistence.EF.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly SibintekDbContext _context;

        public RolesRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<Role> Create(Role role, CancellationToken token = default)
        {
            _context.Roles.Add(role);

            await _context.SaveChangesAsync(token);
            return role;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(c => c.Id == id, token) ?? throw new NotFoundException<Role>(id);

            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Role>> GetAll(CancellationToken token = default)
        {
            return await _context.Roles.AsNoTracking().ToListAsync(token);
        }

        public async Task<Role> GetById(int id, CancellationToken token = default)
        {
            return await _context.Roles.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Role>(id);
        }

        public async Task<Role> GetByName(string name, CancellationToken token = default)
        {
            return await _context.Roles.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Role>();
        }

        public async Task<Role> Update(Role role, CancellationToken token = default)
        {
            var old = await _context.Roles.FirstOrDefaultAsync(c => c.Id == role.Id, token) ?? throw new NotFoundException<Role>(role.Id);
            old.Name = role.Name;

            await _context.SaveChangesAsync(token);

            return role;
        }
    }
}
