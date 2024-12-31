using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace SibintekTask.Infrastructure.Logging
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration loggerConfiguration)
        {
            var elasticsearchUri = context.Configuration["ElasticConfiguration:Uri"];
            var environmentName = context.HostingEnvironment.EnvironmentName;
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-");

            loggerConfiguration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(new ElasticsearchJsonFormatter(), "logs/applogs-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUri))
                {
                    IndexFormat = $"{assemblyName}-{environmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true
                })
                .Enrich.WithProperty("Environment", environmentName)
                .ReadFrom.Configuration(context.Configuration);
        }
    }
}
