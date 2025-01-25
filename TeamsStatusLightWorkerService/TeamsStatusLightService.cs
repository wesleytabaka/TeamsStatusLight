using TeamsStatusLight;

namespace App.WindowsService;

public sealed class TeamsStatusLightService
{
    TeamsStatusLightProgram TeamsStatusLightProgram;

    public TeamsStatusLightService() {
        TeamsStatusLightProgram = new TeamsStatusLightProgram();
    }

    public void Run() {
        TeamsStatusLightProgram.Run();
    }

    public void Stop() {
        // Stop the service
        // TODO
    }
}