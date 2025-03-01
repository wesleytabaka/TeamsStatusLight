using System.ServiceProcess;

namespace App.WindowsService;

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly TeamsStatusLightService _teamsStatusLightService;
    private readonly ILogger<WindowsBackgroundService> _logger;
    private readonly ServiceBase _serviceBase;

    public WindowsBackgroundService(TeamsStatusLightService teamsStatusLightService, ILogger<WindowsBackgroundService> _logger) {
        this._teamsStatusLightService = teamsStatusLightService;
        this._logger = _logger;
        this._serviceBase = new ServiceBaseImplementation(this);
    }

    //private readonly ILogger<Worker> _logger;

    //public Worker(ILogger<Worker> logger)
    //{
    //    _logger = logger;
    //}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void OnPowerEvent(PowerBroadcastStatus powerStatus)
    {
        switch (powerStatus)
        {
            case PowerBroadcastStatus.Suspend:
                _logger.LogInformation("System is about to be suspended.");
                this._teamsStatusLightService.EnterSuspendState();
                break;
        }
    }

    private class ServiceBaseImplementation : ServiceBase
    {
        private readonly WindowsBackgroundService _backgroundService;

        public ServiceBaseImplementation(WindowsBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            _backgroundService.OnPowerEvent(powerStatus);
            return base.OnPowerEvent(powerStatus);
        }
    }
}
