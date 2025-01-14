using Microsoft.EntityFrameworkCore;
using SibintekTask.Core.Exceptions;
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
        private readonly SibintekDbContext _context;

        public RepairTypesRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<RepairType> Create(RepairType type, CancellationToken token = default)
        {
            _context.RepairTypes.Add(type);

            await _context.SaveChangesAsync(token);
            return type;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var type = await _context.RepairTypes.FirstOrDefaultAsync(c => c.Id == id, token) ?? throw new NotFoundException<RepairType>(id);

            _context.RepairTypes.Remove(type);
            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<RepairType>> GetAll(CancellationToken token = default)
        {
            return await _context.RepairTypes.AsNoTracking().ToListAsync(token);
        }

        public async Task<RepairType?> GetById(int id, CancellationToken token = default)
        {
            return await _context.RepairTypes.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<RepairType>(id);
        }

        public async Task<RepairType?> GetByName(string name, CancellationToken token = default)
        {
            return await _context.RepairTypes.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token);
        }

        public async Task<RepairType?> Update(RepairType type, CancellationToken token = default)
        {
            var old = await _context.RepairTypes.FirstOrDefaultAsync(c => c.Id == type.Id, token) ?? throw new NotFoundException<RepairType>(type.Id);
            old.Name = type.Name;

            await _context.SaveChangesAsync(token);

            return type;
        }
    }
}
