using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SibintekTask.Application.Auth;
using SibintekTask.Application.Interfaces;
using SibintekTask.Application.Services;
using SibintekTask.Persistence.EF;

namespace SibintekTask.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgreSQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<SibintekDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("LocalPostgreSQL")));

            return services;
        }

        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<IMarksService, MarksService>();
            services.AddScoped<IRepairsService, RepairsService>();
            services.AddScoped<IRepairTypesService, RepairTypesService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IVehiclesService, VehiclesService>();
            services.AddScoped<IReportsService, ReportsService>();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();


            return services;
        }
    }
}
