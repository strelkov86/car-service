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
    public class RepairsRepository : IRepairsRepository
    {
        private readonly SibintekDbContext _context;

        public RepairsRepository(SibintekDbContext c)
        {
            _context = c;
        }

        public async Task<Repair> Create(Repair repair, CancellationToken token = default)
        {
            _context.Repairs.Add(repair);

            await _context.SaveChangesAsync(token);
            await LoadRepairReferences(repair, token);
            return repair;
        }

        public async Task<int> Delete(int id, CancellationToken token = default)
        {
            var repair = await _context.Repairs.FirstOrDefaultAsync(r => r.Id == id, token) ?? throw new NotFoundException<Repair>(id);
            _context.Repairs.Remove(repair);
            return await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<Repair>?> GetAll(CancellationToken token = default)
        {
            return await _context.Repairs
                .Include(r => r.RepairType)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.Mark)
                .Include(r => r.Executor)
                .Include(r => r.Customer)
                .AsNoTracking().ToListAsync(token);
        }

        public async Task<Repair> GetById(int id, CancellationToken token = default)
        {
            return await _context.Repairs.Where(r => r.Id == id)
                .Include(r => r.RepairType)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.Mark)
                .Include(r => r.Executor)
                .Include(r => r.Customer)
                .AsNoTracking().FirstOrDefaultAsync(token) ?? throw new NotFoundException<Repair>(id);
        }

        public async Task<RepairReport?> GetReportByCustomer(int customerId, DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            var repairs = _context.Repairs.Where(r => r.CustomerId == customerId).HandlePeriod(startDate, endDate);
            if (!await repairs.AnyAsync(token)) return null;

            var report = await GetRepairCountAndSum(repairs, token);
            report.StartDate = startDate;
            report.EndDate = endDate;
            return report;
        }

        public async Task<RepairReport?> GetReportByExecutor(int executorId, DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            var repairs = _context.Repairs.Where(r => r.ExecutorId == executorId).HandlePeriod(startDate, endDate);
            if (!repairs.Any()) return null;

            var report = await GetRepairCountAndSum(repairs, token);
            report.StartDate = startDate;
            report.EndDate = endDate;
            return report;
        }

        public async Task<RepairReport?> GetTotalReport(DateTime? startDate, DateTime? endDate, CancellationToken token = default)
        {
            var repairs = _context.Repairs.HandlePeriod(startDate, endDate);
            if (!repairs.Any()) return null;

            var report = await GetRepairCountAndSum(repairs, token);
            report.StartDate = startDate;
            report.EndDate = endDate;
            return report;
        }

        private static async Task<RepairReport> GetRepairCountAndSum(IQueryable<Repair> repairs, CancellationToken token = default)
        {
            var repairCount = await repairs.CountAsync(token);
            var repairSum = await repairs.SumAsync(r => r.Cost, token);

            return new RepairReport
            {
                RepairCount = repairCount,
                TotalSumRubles = repairSum
            };
        }

        public async Task<VehicleReport?> GetReportByVehicle(int vehicleId, DateTime? startDate, DateTime? endDate, int? userId = null, CancellationToken token = default)
        {
            var repairs = _context.Repairs.Where(r => r.VehicleId == vehicleId).HandlePeriod(startDate, endDate);

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

        public async Task<Repair> Update(Repair repair, CancellationToken token = default)
        {
            var old = await _context.Repairs.FirstOrDefaultAsync(r => r.Id == repair.Id, token) ?? throw new NotFoundException<Repair>(repair.Id);

            old.AcceptedAt = repair.AcceptedAt;
            old.FinishedAt = repair.FinishedAt;
            old.Cost = repair.Cost;
            old.RepairTypeId = repair.RepairTypeId;
            old.Mileage = repair.Mileage;
            old.CustomerId = repair.CustomerId;
            old.ExecutorId = repair.ExecutorId;
            old.VehicleId = repair.VehicleId;

            await _context.SaveChangesAsync(token);
            await LoadRepairReferences(old, token);
            return old;
        }

        public async Task<Repair> FinishRepair(int id, CancellationToken token = default)
        {
            var old = await _context.Repairs.FirstOrDefaultAsync(r => r.Id == id, token) ?? throw new NotFoundException<Repair>(id);

            old.FinishedAt = DateTime.Now;
            await _context.SaveChangesAsync(token);
            await LoadRepairReferences(old, token);
            return old;
        }

        private async Task LoadRepairReferences(Repair repair, CancellationToken token = default)
        {
            var type = _context.Entry(repair).Reference(r => r.RepairType).LoadAsync(token);
            var customer = _context.Entry(repair).Reference(r => r.Customer).LoadAsync(token);
            var executor = _context.Entry(repair).Reference(r => r.Executor).LoadAsync(token);

            await _context.Entry(repair).Reference(r => r.Vehicle).LoadAsync(token);
            var mark = _context.Entry(repair.Vehicle).Reference(v => v.Mark).LoadAsync(token);

            await Task.WhenAll(type, customer, executor, mark);
        }
    }
}
