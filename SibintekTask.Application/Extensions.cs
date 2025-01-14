using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SibintekTask.Application.Auth;
using SibintekTask.Application.Interfaces;
using SibintekTask.Application.Services;
using SibintekTask.Persistence.EF;
using System;

namespace SibintekTask.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddPostgreSQL(this IServiceCollection services, IConfiguration configuration)
        {
            var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
            var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");

            var connectionString = dbUser == null && dbPassword == null && dbName == null
                ? configuration.GetConnectionString("LocalPostgreSQL")
                : $"Host=postgresql;Port=5432;Database={dbName};Username={dbUser};Password={dbPassword};";
            services.AddDbContext<SibintekDbContext>(options =>
                options.UseNpgsql(connectionString));

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
