using Serilog;
using Serilog.Events;

namespace LibraryAPI.Configuration;

public static class LoggerConfigurator
{
    public static void ConfigureLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            // Write logs to console
            .WriteTo.Async(a => a.Console())
            
            // Write all logs to file
            .WriteTo.Async(a => a.File("logs/all-logs-.txt", rollingInterval: RollingInterval.Day))
            
            // Write only errors to file
            .WriteTo.Async(a => a.File("logs/error-logs-.txt", rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error))
            
            // Write only warning to file 
            .WriteTo.Async(a => a.File("logs/warning-logs-.txt", rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning))
            
            // Write only info to file
            .WriteTo.Async(a => a.File("logs/info-logs-.txt", rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information))
            .CreateLogger();
    }
}