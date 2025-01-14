using Microsoft.EntityFrameworkCore;
using SibintekTask.Core.Exceptions;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Persistence.EF.Repositories
{
    public class MarksRepository : IMarksRepository
    {
        private readonly SibintekDbContext _context;

        public MarksRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<Mark> Create(Mark mark, CancellationToken token = default)
        {
            _context.Marks.Add(mark);
            await _context.SaveChangesAsync(token);
            return mark;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var mark = await _context.Marks.FirstOrDefaultAsync(c => c.Id == id, token) ?? throw new NotFoundException<Mark>(id);
            _context.Marks.Remove(mark);
            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Mark>> GetAll(CancellationToken token = default)
        {
            return await _context.Marks.AsNoTracking().ToListAsync(token);
        }

        public async Task<Mark> GetById(int id, CancellationToken token = default)
        {
            return await _context.Marks.Where(c => c.Id == id).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Mark>(id);
        }

        public async Task<Mark> GetByName(string name, CancellationToken token = default)
        {
            return await _context.Marks.Where(c => c.Name == name).AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Mark>();
        }

        public async Task<Mark> Update(Mark mark, CancellationToken token = default)
        {
            var old = await _context.Marks.FirstOrDefaultAsync(c => c.Id == mark.Id, token) ?? throw new NotFoundException<Mark>(mark.Id);

            old.Name = mark.Name;
            await _context.SaveChangesAsync(token);
            return mark;
        }
    }
}
