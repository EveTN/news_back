using System;
using System.Reflection;
using Core.ConfigureOptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                var elkOptions = new ElkOptions();
                configuration.GetSection("ElkOptions").Bind(elkOptions);

                if (elkOptions.ElasticSearch != default)
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elkOptions.ElasticSearch))
                        {
                            AutoRegisterTemplate = true,
                            IndexFormat =
                                $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{DateTime.UtcNow:yyyy-MM}"
                        })
                        .Enrich.WithProperty("Environment", environment)
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}