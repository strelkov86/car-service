using AutoMapper;
using SibintekTask.Application.Interfaces;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using System;
using System.Threading.Tasks;

namespace SibintekTask.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IRepairsRepository _repairs;
        public ReportsService(IRepairsRepository r)
        {
            _repairs = r;
        }

        public Task<RepairReport> GetReportByCustomerId(int customerId, DateTime? startDate, DateTime? endDate)
        {
            return _repairs.GetReportByCustomer(customerId, startDate, endDate);
        }

        public Task<RepairReport> GetReportByExecutorId(int executorId, DateTime? startDate, DateTime? endDate)
        {
            return _repairs.GetReportByExecutor(executorId, startDate, endDate);
        }

        public Task<VehicleReport> GetReportByVehicleId(int vehicleId, DateTime? startDate, DateTime? endDate, int? userId = null)
        {
            return _repairs.GetReportByVehicle(vehicleId, startDate, endDate, userId);
        }

        public Task<RepairReport> GetTotalReport(DateTime? startDate, DateTime? endDate)
        {
            return _repairs.GetTotalReport(startDate, endDate);
        }
    }
}
