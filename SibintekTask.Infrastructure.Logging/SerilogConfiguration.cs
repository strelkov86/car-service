using Serilog.Events;
using Serilog;
using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Serilog.Formatting.Json;

namespace SibintekTask.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            var seqHost = Environment.GetEnvironmentVariable("SEQ_HOST");
            var seqUri = string.IsNullOrEmpty(seqHost) ? context.Configuration["SeqConfiguration:Uri"] : $"http://{seqHost}:5341";
            var environmentName = context.HostingEnvironment.EnvironmentName;

            loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .WriteTo.Console()
                .WriteTo.File(new JsonFormatter(), "logs/applogs-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Seq(seqUri)
                .ReadFrom.Configuration(context.Configuration);
        }
    }
}
