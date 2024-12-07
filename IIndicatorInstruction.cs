using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace TeamsStatusLight
{
    public interface IIndicatorInstruction
    {
        int r {  get; set; }
        int g { get; set; }
        int b { get; set; }
        Effect effect { get; set; }
        int effectRate { get; set; }
        Transition transition { get; set; }
        int transitionDuration { get; set; }
        [DllImport("TeamsStatusLight.dll")]
        static extern Dictionary<string, IIndicatorInstruction> DeserializeIndicatorInstructionsFromConfig(IConfigurationSection config);
    }
}
