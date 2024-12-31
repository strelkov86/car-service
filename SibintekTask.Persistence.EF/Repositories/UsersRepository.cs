using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Persistence.EF.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public UsersRepository(IDbContextFactory<SibintekDbContext> f, ILogger<UsersRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<bool> AttachRoleToUser(int userId, int roleId, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var user = await db.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId, token);
                var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId, token);
                if (user.Roles.Contains(role)) return false;

                user.Roles.Add(role);

                await db.SaveChangesAsync(token);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Create(User user, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            db.Users.Add(user);

            try
            {
                await db.SaveChangesAsync(token);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(c => c.Id == id, token);
                if (user is null) return 0;

                db.Users.Remove(user);

                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Users.Include(u => u.Roles).AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Users.Include(u => u.Roles).Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> GetByITN(string itn, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Users.Include(u => u.Roles).Where(c => c.ITN.Contains(itn)).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>?> GetByRole(int roleId, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var role = await db.Roles.FindAsync(new object[] { roleId }, cancellationToken: token);
                if (role is null) return null;
                return await db.Users.Include(u => u.Roles).Where(u => u.Roles.Contains(role)).ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveRoleFromUser(int userId, int roleId, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var user = await db.Users.Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id == userId, token);

                if (user is null) return false;

                var roleToRemove = user.Roles.FirstOrDefault(r => r.Id == roleId);
                if (roleToRemove is null) return false;

                user.Roles.Remove(roleToRemove);

                await db.SaveChangesAsync(token);

                _logger.LogInformation($"Роль с ID {roleId} успешно удалена у пользователя с ID {userId}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> SetPasswordHash(int userId, string passwordHash, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var old = await db.Users.Include(u => u.Roles).FirstOrDefaultAsync(c => c.Id == userId, token);
                if (old is null) return null;

                old.PasswordHash = passwordHash;

                await db.SaveChangesAsync(token);
                return old;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> Update(User user, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var old = await db.Users.FirstOrDefaultAsync(c => c.Id == user.Id, token);
                if (old is null) return null;

                old.Name = user.Name;
                old.Surname = user.Surname;
                old.ITN = user.ITN;

                await db.SaveChangesAsync(token);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
