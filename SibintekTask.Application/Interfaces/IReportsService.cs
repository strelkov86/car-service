using System;
using System.Threading.Tasks;
using SibintekTask.Core.Models;

namespace SibintekTask.Application.Interfaces
{
    public interface IReportsService
    {
        public Task<RepairReport> GetReportByCustomerId(int customerId, DateTime? startDate, DateTime? endDate);
        public Task<RepairReport> GetReportByExecutorId(int executorId, DateTime? startDate, DateTime? endDate);
        public Task<RepairReport> GetTotalReport(DateTime? startDate, DateTime? endDate);
        public Task<VehicleReport> GetReportByVehicleId(int vehicleId, DateTime? startDate, DateTime? endDate, int? userId = null);
    }
}
