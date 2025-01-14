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
    public class UsersRepository : IUsersRepository
    {
        private readonly SibintekDbContext _context;

        public UsersRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<bool> AttachRoleToUser(int userId, int roleId, CancellationToken token = default)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId, token) ?? throw new NotFoundException<User>(userId); ;
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId, token) ?? throw new NotFoundException<Role>(roleId);
            if (user.Roles.Contains(role)) return false;

            user.Roles.Add(role);

            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<User> Create(User user, CancellationToken token = default)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync(token);
            return user;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id, token) ?? throw new NotFoundException<User>(id);

            _context.Users.Remove(user);

            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken token = default)
        {
            return await _context.Users.Include(u => u.Roles).AsNoTracking().ToListAsync(token);
        }

        public async Task<User?> GetById(int id, CancellationToken token = default)
        {
            return await _context.Users.Include(u => u.Roles).Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<User>(id);
        }

        public async Task<User?> GetByITN(string itn, CancellationToken token = default)
        {
            return await _context.Users.Include(u => u.Roles).Where(c => c.ITN.Contains(itn)).AsNoTracking().FirstOrDefaultAsync(token);
        }

        public async Task<IEnumerable<User>?> GetByRole(int roleId, CancellationToken token = default)
        {
            var role = await _context.Roles.FindAsync(new object[] { roleId }, cancellationToken: token);
            if (role is null) return null;
            return await _context.Users.Include(u => u.Roles).Where(u => u.Roles.Contains(role)).ToListAsync(token);
        }

        public async Task<bool> RemoveRoleFromUser(int userId, int roleId, CancellationToken token = default)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId, token)
                 ?? throw new NotFoundException<User>(userId);

            var roleToRemove = user.Roles.FirstOrDefault(r => r.Id == roleId) ?? throw new NotFoundException<Role>(roleId);

            user.Roles.Remove(roleToRemove);

            await _context.SaveChangesAsync(token);

            return true;
        }

        public async Task<User> SetPasswordHash(int userId, string passwordHash, CancellationToken token = default)
        {
            var old = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(c => c.Id == userId, token) ?? throw new NotFoundException<User>(userId);

            old.PasswordHash = passwordHash;

            await _context.SaveChangesAsync(token);
            return old;
        }

        public async Task<User?> Update(User user, CancellationToken token = default)
        {
            var old = await _context.Users.FirstOrDefaultAsync(c => c.Id == user.Id, token) ?? throw new NotFoundException<User>(user.Id);

            old.Name = user.Name;
            old.Surname = user.Surname;
            old.ITN = user.ITN;

            await _context.SaveChangesAsync(token);
            
            return user;
        }
    }
}
