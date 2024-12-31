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
    public class MarksRepository : IMarksRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public MarksRepository(IDbContextFactory<SibintekDbContext> f, ILogger<MarksRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<Mark> Create(Mark mark, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                db.Marks.Add(mark);
                await db.SaveChangesAsync(token);
                return mark;
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
                var mark = await db.Marks.FirstOrDefaultAsync(c => c.Id == id, token);
                if (mark is null) return 0;

                db.Marks.Remove(mark);
                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Mark>> GetAll(CancellationToken token = default)
        {
            //_logger.LogCritical("test log critical");
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Marks.AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Mark?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Marks.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Mark?> GetByName(string name, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Marks.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Mark?> Update(Mark mark, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var old = await db.Marks.FirstOrDefaultAsync(c => c.Id == mark.Id, token);
                if (old is null) return null;

                old.Name = mark.Name;
                await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
            return mark;
        }
    }
}
