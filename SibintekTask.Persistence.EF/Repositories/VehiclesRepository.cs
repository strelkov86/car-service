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
    public class VehiclesRepository : IVehiclesRepository
    {
        private readonly SibintekDbContext _context;

        public VehiclesRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<Vehicle> Create(Vehicle vehicle, CancellationToken token = default)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync(token);
            await _context.Entry(vehicle).Reference(v => v.Mark).LoadAsync(token);
            return vehicle;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == id, token) ?? throw new NotFoundException<Vehicle>(id);

            _context.Vehicles.Remove(vehicle);
            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Vehicle>> GetAll(CancellationToken token = default)
        {
            return await _context.Vehicles.Include(v => v.Mark).AsNoTracking().ToListAsync(token);
        }

        public async Task<Vehicle?> GetById(int id, CancellationToken token = default)
        {
            return await _context.Vehicles
                .Include(v => v.Mark)
                .Where(c => c.Id == id)
                .AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Vehicle>(id); ;
        }

        public async Task<Vehicle?> GetByNumberPlate(string numberPlate, CancellationToken token = default)
        {
            return await _context.Vehicles
                .Include(v => v.Mark)
                .Where(c => c.NumberPlate!.ToLower().Contains(numberPlate.ToLower()))
                .AsNoTracking().FirstOrDefaultAsync(token);
        }

        public async Task<Vehicle?> GetByPlateAndMark(string numberPlate, int markId, CancellationToken token = default)
        {
            return await _context.Vehicles
                .Include(v => v.Mark)
                .Where(c => c.NumberPlate!.ToLower().Contains(numberPlate.ToLower()))
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.NumberPlate == numberPlate && v.MarkId == markId, token);
        }

        public async Task<Vehicle?> Update(Vehicle vehicle, CancellationToken token = default)
        {
            var old = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == vehicle.Id, token) ?? throw new NotFoundException<Vehicle>(vehicle.Id);

            old.NumberPlate = vehicle.NumberPlate;
            old.MarkId = vehicle.MarkId;

            await _context.SaveChangesAsync(token);
            await _context.Entry(old).Reference(v => v.Mark).LoadAsync(token);

            return old;
        }
    }
}
