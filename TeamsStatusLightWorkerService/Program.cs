using App.WindowsService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

public class Program
{
    const string ServiceName = "TeamsStatusLight Service";
    public static void Main(string[] args)
    {
        if (args.Length > 0) {
            string exePath = Path.Combine(AppContext.BaseDirectory, "TeamsStatusLightWorkerService.exe");

            if (args[0] == "/Install") {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc", $"create \"{ServiceName}\" binPath= \"{exePath}\"");
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = psi;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }
            if (args[0] == "/Uninstall") {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("sc", $"stop \"{ServiceName}\"");
                psi.RedirectStandardOutput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = psi;
                proc.Start();
                string result = proc.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
                
                System.Diagnostics.ProcessStartInfo psi2 = new System.Diagnostics.ProcessStartInfo("sc", $"delete \"{ServiceName}\"");
                psi2.RedirectStandardOutput = true;
                psi2.UseShellExecute = false;
                psi2.CreateNoWindow = true;
                System.Diagnostics.Process proc2 = new System.Diagnostics.Process();
                proc2.StartInfo = psi2;
                proc2.Start();
                string result2 = proc2.StandardOutput.ReadToEnd();
                Console.WriteLine(result2);
            }
        }
        else {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddWindowsService(options => {
                options.ServiceName = ServiceName;
            });

            LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

            builder.Services.AddSingleton<TeamsStatusLightService>();
            builder.Services.AddHostedService<WindowsBackgroundService>();

            var host = builder.Build();
            host.Run();
        }
    }
}
