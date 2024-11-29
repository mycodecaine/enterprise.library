
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;

namespace Cdcn.Enterprise.Library.Logging
{
    public class Logging
    {
        public static ILogger Logger { get; private set; }
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
        (context, loggerConfiguration) =>
        {
            var env = context.HostingEnvironment;
            loggerConfiguration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console().WriteTo.Debug();
            if (context.HostingEnvironment.IsDevelopment())
            {
                loggerConfiguration.MinimumLevel.Override(env.ApplicationName, LogEventLevel.Information);

            }

            var elasticUrl = context.Configuration.GetValue<string>("Elastic:Url"); ; // Define later
            if (!string.IsNullOrEmpty(elasticUrl))
            {
                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(elasticUrl))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                        IndexFormat = $"{env.ApplicationName}-Logs-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = LogEventLevel.Debug
                    });
            }           
        };
    }
}
