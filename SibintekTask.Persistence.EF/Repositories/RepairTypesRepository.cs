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
    public class RepairTypesRepository : IRepairTypesRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public RepairTypesRepository(IDbContextFactory<SibintekDbContext> f, ILogger<RepairTypesRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<RepairType> Create(RepairType type, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            db.RepairTypes.Add(type);

            try
            {
                await db.SaveChangesAsync(token);
                return type;
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
                var type = await db.RepairTypes.FirstOrDefaultAsync(c => c.Id == id, token);
                if (type is null) return 0;

                db.RepairTypes.Remove(type);
                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RepairType>> GetAll(CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.RepairTypes.AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairType?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.RepairTypes.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairType?> GetByName(string name, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.RepairTypes.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairType?> Update(RepairType type, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                var old = await db.RepairTypes.FirstOrDefaultAsync(c => c.Id == type.Id, token);
                if (old is null) return null;

                old.Name = type.Name;
                await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return type;
        }
    }
}
