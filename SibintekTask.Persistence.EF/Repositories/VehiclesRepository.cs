using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class VehiclesRepository : IVehiclesRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public VehiclesRepository(IDbContextFactory<SibintekDbContext> f, ILogger<VehiclesRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<Vehicle> Create(Vehicle vehicle, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                db.Vehicles.Add(vehicle);
                await db.SaveChangesAsync(token);
                await db.Entry(vehicle).Reference(v => v.Mark).LoadAsync(token);
                return vehicle;
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
                var vehicle = await db.Vehicles.FirstOrDefaultAsync(c => c.Id == id, token);
                if (vehicle is null) return 0;

                db.Vehicles.Remove(vehicle);
                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Vehicle>> GetAll(CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Vehicles.Include(v => v.Mark).AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Vehicle?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Vehicles
                    .Include(v => v.Mark)
                    .Where(c => c.Id == id)
                    .AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Vehicle?> GetByNumberPlate(string numberPlate, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Vehicles
                    .Include(v => v.Mark)
                    .Where(c => c.NumberPlate!.ToLower().Contains(numberPlate.ToLower()))
                    .AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Vehicle?> GetByPlateAndMark(string numberPlate, int markId, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                return await db.Vehicles
                    .Include(v => v.Mark)
                    .Where(c => c.NumberPlate!.ToLower().Contains(numberPlate.ToLower()))
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.NumberPlate == numberPlate && v.MarkId == markId, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Vehicle?> Update(Vehicle vehicle, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var old = await db.Vehicles.FirstOrDefaultAsync(c => c.Id == vehicle.Id, token);
                if (old is null) return null;

                old.NumberPlate = vehicle.NumberPlate;
                old.MarkId = vehicle.MarkId;
                await db.SaveChangesAsync(token);
                await db.Entry(old).Reference(v => v.Mark).LoadAsync(token);
                return old;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
