using App.WindowsService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

public class Program
{
    public static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddWindowsService(options => { 
            options.ServiceName = "TeamsStatusLight Service";
        });

        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

        builder.Services.AddSingleton<TeamsStatusLightService>();
        builder.Services.AddHostedService<WindowsBackgroundService>();

        var host = builder.Build();
        host.Run();
    }
}
