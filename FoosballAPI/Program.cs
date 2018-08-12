using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using System.IO;
using Serilog.Events;

namespace FoosballAPI
{
    public class Program
    {
        public static int Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                Log.Information("Starting...");

                WebHost.CreateDefaultBuilder(args)
                    .UseSerilog()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error has occured!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(LogEventLevel.Warning)
                .WriteTo.RollingFile($"logs\\{nameof(FoosballAPI)}-All-{{Date}}.txt", LogEventLevel.Debug)
                .WriteTo.RollingFile($"logs\\{nameof(FoosballAPI)}-Error-{{Date}}.txt", LogEventLevel.Error)
                .CreateLogger();
        }
    }

}

