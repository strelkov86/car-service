using SibintekTask.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SibintekTask.Core.Interfaces
{
    public interface IRepairsRepository
    {
        public Task<IEnumerable<Repair>?> GetAll(CancellationToken token = default);
        public Task<Repair> GetById(int id, CancellationToken token = default);
        public Task<Repair> Create (Repair repair, CancellationToken token = default);
        public Task<Repair> Update (Repair repair, CancellationToken token = default);
        public Task<int> Delete (int id, CancellationToken token = default);

        public Task<Repair> FinishRepair (int id, CancellationToken token = default);

        public Task<RepairReport?> GetReportByCustomer(int customerId, DateTime? startDate, DateTime? endDate, CancellationToken token = default);
        public Task<RepairReport?> GetReportByExecutor(int executorId, DateTime? startDate, DateTime? endDate, CancellationToken token = default);
        public Task<RepairReport?> GetTotalReport(DateTime? startDate, DateTime? endDate, CancellationToken token = default);
        public Task<VehicleReport?> GetReportByVehicle(int vehicleId, DateTime? startDate, DateTime? endDate, int? userId = null, CancellationToken token = default);
    }
}
