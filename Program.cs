using Microsoft.Extensions.Configuration;
using TeamsStatusLight;

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");
IConfigurationRoot Configuration = configurationBuilder.Build();
string _portName = Configuration["COMPort"] ?? "COM1";

TeamsStatus teamsStatus = new TeamsStatus();
Presence presence;

Indicator _indicator = new Indicator(_portName);
IndicatorInstruction _initialInstruction = new IndicatorInstruction(0, 0, 0); // Off
_indicator.SetIndicator(_initialInstruction);

Dictionary<string, IndicatorInstruction> indicatorInstructionMapping = IndicatorInstruction.DeserializeIndicatorInstructionsFromConfig(Configuration.GetSection("IndicatorInstructionMapping"));

int _counter = 0;

IndicatorInstruction currentInstruction;
IndicatorInstruction newInstruction;

AutoResetEvent autoEvent = new AutoResetEvent(false);
Timer t = new Timer(CheckPresenceAndSetIndicator, autoEvent, 0, 5000);
autoEvent.WaitOne();

void CheckPresenceAndSetIndicator(Object stateinfo) {
    Console.WriteLine("Invoked CheckPresenceAndSetIndicator");
    AutoResetEvent autoEvent = (AutoResetEvent) stateinfo;

    presence = (Presence)teamsStatus.getPresence();
    Console.WriteLine("Availability: " + presence.availability);

    //_indicator.SetIndicator(colors.ToArray()[_counter % colors.Count].Value); // Debug: Cycle through colors
    IndicatorInstruction color;
    indicatorInstructionMapping.TryGetValue(presence.availability, out color);
    _indicator.SetIndicator(color);

    _counter = (_counter + 1) % indicatorInstructionMapping.Count;

    autoEvent.Reset();

}


