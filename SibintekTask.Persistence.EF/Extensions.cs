using Microsoft.Extensions.DependencyInjection;
using SibintekTask.Core.Interfaces;
using SibintekTask.Core.Models;
using SibintekTask.Persistence.EF.Repositories;
using System.Linq;
using System;

namespace SibintekTask.Persistence.EF
{
    public static class Extensions
    {
        public static IServiceCollection AddEFCoreRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IMarksRepository, MarksRepository>();
            services.AddScoped<IRepairTypesRepository, RepairTypesRepository>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();
            services.AddScoped<IRepairsRepository, RepairsRepository>();

            return services;
        }

        public static IQueryable<Repair> HandlePeriod(this IQueryable<Repair> repairs, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                repairs = repairs.Where(r => r.AcceptedAt >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                repairs = repairs.Where(r => (r.FinishedAt == null || r.FinishedAt <= endDate.Value) && r.AcceptedAt <= endDate.Value);
            }
            return repairs;
        }
    }
}
