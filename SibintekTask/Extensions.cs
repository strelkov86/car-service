using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SibintekTask.API.Middlewares;
using SibintekTask.Application.Auth;
using System.Text;
using System.Threading.Tasks;

namespace SibintekTask.API
{
    public static class Extensions
    {
        public static IApplicationBuilder UseGlobalRoutePrefix(this IApplicationBuilder app, string routePrefix)
        {
            return app.UseMiddleware<GlobalRoutePrefixMiddleware>(routePrefix);
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration jwtOptions)
        {
            var jwtKey = jwtOptions.GetSection("JwtOptions:Key").Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };

                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["sibintek-cookie"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
