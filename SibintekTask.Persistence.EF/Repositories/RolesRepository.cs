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
    public class RolesRepository : IRolesRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public RolesRepository(IDbContextFactory<SibintekDbContext> f, ILogger<RolesRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<Role> Create(Role role, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            db.Roles.Add(role);

            try
            {
                await db.SaveChangesAsync(token);
                return role;
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
                var role = await db.Roles.FirstOrDefaultAsync(c => c.Id == id, token);
                if (role is null) return 0;

                db.Roles.Remove(role);
                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Role>> GetAll(CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.Roles.AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Role?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.Roles.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Role?> GetByName(string name, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.Roles.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Role?> Update(Role role, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                var old = await db.Roles.FirstOrDefaultAsync(c => c.Id == role.Id, token);
                if (old is null) return null;

                old.Name = role.Name;
                await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return role;
        }
    }
}
