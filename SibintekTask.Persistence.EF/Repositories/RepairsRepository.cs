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
    public class RepairsRepository : IRepairsRepository
    {
        private readonly IDbContextFactory<SibintekDbContext> _dbContextFactory;
        private readonly ILogger _logger;

        public RepairsRepository(IDbContextFactory<SibintekDbContext> f, ILogger<RepairsRepository> l)
        {
            _dbContextFactory = f;
            _logger = l;
        }

        public async Task<Repair> Create(Repair repair, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            db.Repairs.Add(repair);

            try
            {
                await db.SaveChangesAsync(token);
                await LoadRepairReferences(db, repair, token);
                return repair;
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
                var repair = await db.Repairs.FirstOrDefaultAsync(r => r.Id == id, token);
                if (repair is null) return 0;

                db.Repairs.Remove(repair);
                return await db.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Repair>?> GetAll(CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.Repairs
                    .Include(r => r.RepairType)
                    .Include(r => r.Vehicle)
                        .ThenInclude(v => v.Mark)
                    .Include(r => r.Executor)
                    .Include(r => r.Customer)
                    .AsNoTracking().ToListAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Repair?> GetById(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                return await db.Repairs.Where(r => r.Id == id)
                    .Include(r => r.RepairType)
                    .Include(r => r.Vehicle)
                        .ThenInclude(v => v.Mark)
                    .Include(r => r.Executor)
                    .Include(r => r.Customer)
                    .AsNoTracking().FirstOrDefaultAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairReport?> GetReportByCustomer(int customerId, DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var repairs = db.Repairs.Where(r => r.CustomerId == customerId).HandlePeriod(startDate, endDate);
                if (!repairs.Any()) return null;

                var report = await GetRepairCountAndSum(repairs, token);
                report.StartDate = startDate;
                report.EndDate = endDate;
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairReport?> GetReportByExecutor(int executorId, DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var repairs = db.Repairs.Where(r => r.ExecutorId == executorId).HandlePeriod(startDate, endDate);
                if (!repairs.Any()) return null;

                var report = await GetRepairCountAndSum(repairs, token);
                report.StartDate = startDate;
                report.EndDate = endDate;
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RepairReport?> GetTotalReport(DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var repairs = db.Repairs.HandlePeriod(startDate, endDate);
                if (!repairs.Any()) return null;

                var report = await GetRepairCountAndSum(repairs, token);
                report.StartDate = startDate;
                report.EndDate = endDate;
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private static async Task<RepairReport?> GetRepairCountAndSum(IQueryable<Repair> repairs, CancellationToken token = default)
        {
            var repairCount = repairs.CountAsync(token);
            var repairSum = repairs.SumAsync(r => r.Cost, token);
            await Task.WhenAll(repairCount, repairSum);

            return new RepairReport
            {
                RepairCount = repairCount.Result,
                TotalSumRubles = repairSum.Result
            };
        }

        public async Task<VehicleReport?> GetReportByVehicle(int vehicleId, DateTime? startDate, DateTime? endDate, int? userId = null, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();
            try
            {
                var repairs = db.Repairs.Where(r => r.VehicleId == vehicleId).HandlePeriod(startDate, endDate);

                if (userId.HasValue) repairs = repairs.Where(r => r.CustomerId == userId.Value);

                if (!await repairs.AnyAsync(token)) return null;

                var repairSum = await repairs.SumAsync(r => r.Cost, token);

                var orderedRepairMileages = await repairs.OrderBy(r => r.AcceptedAt).Select(r => r.Mileage).ToListAsync(token);
                var miliagePerPeriod = orderedRepairMileages.Any() ? orderedRepairMileages.Last() - orderedRepairMileages.First() : 0;

                return new VehicleReport
                {
                    TotalSumRubles = repairSum,
                    MiliagesPerPeriodInKm = miliagePerPeriod,
                    StartDate = startDate,
                    EndDate = endDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Repair?> Update(Repair repair, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                var old = await db.Repairs.FirstOrDefaultAsync(r => r.Id == repair.Id, token);
                if (old is null) return null;

                old.AcceptedAt = repair.AcceptedAt;
                old.FinishedAt = repair.FinishedAt;
                old.Cost = repair.Cost;
                old.RepairTypeId = repair.RepairTypeId;
                old.Mileage = repair.Mileage;
                old.CustomerId = repair.CustomerId;
                old.ExecutorId = repair.ExecutorId;
                old.VehicleId = repair.VehicleId;

                await db.SaveChangesAsync(token);
                await LoadRepairReferences(db, old, token);
                return old;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Repair?> FinishRepair(int id, CancellationToken token = default)
        {
            await using var db = _dbContextFactory.CreateDbContext();

            try
            {
                var old = await db.Repairs.FirstOrDefaultAsync(r => r.Id == id, token);
                if (old is null) return null;

                old.FinishedAt = DateTime.Now;
                await db.SaveChangesAsync(token);
                await LoadRepairReferences(db, old, token);
                return old;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task LoadRepairReferences(DbContext db, Repair repair, CancellationToken token = default)
        {
            var type = db.Entry(repair).Reference(r => r.RepairType).LoadAsync(token);
            var customer = db.Entry(repair).Reference(r => r.Customer).LoadAsync(token);
            var executor = db.Entry(repair).Reference(r => r.Executor).LoadAsync(token);

            await db.Entry(repair).Reference(r => r.Vehicle).LoadAsync(token);
            var mark = db.Entry(repair.Vehicle).Reference(v => v.Mark).LoadAsync(token);

            await Task.WhenAll(type, customer, executor, mark);
        }
    }
}
