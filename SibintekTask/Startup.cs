using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SibintekTask.API.Middlewares;
using SibintekTask.Application;
using SibintekTask.Application.Auth;
using SibintekTask.Persistence.EF;

namespace SibintekTask.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));
            services.AddPostgreSQL(Configuration);
            services.AddEFCoreRepositories();
            services.AddBusinessLogic();

            services.AddAutoMapper(typeof(Mapping));

            services.AddControllers();

            services.AddJwtAuthentication(Configuration);

            services.AddSwaggerGen(opt =>
            {
                //opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = @"Введите JWT токен авторизации",
                //    Name = "Authorization",
                //    In = ParameterLocation.Cookie,
                //    Type = SecuritySchemeType.ApiKey,
                //    BearerFormat = "JWT",
                //    Scheme = "Bearer"
                //});

                //opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            },
                //        },
                //        new List<string>()
                //    }
                //});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalRoutePrefix("/api/");
            app.UsePathBase(new PathString("/api"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
