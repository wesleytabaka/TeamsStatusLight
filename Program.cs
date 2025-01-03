﻿using Microsoft.Extensions.Configuration;
using TeamsStatusLight;

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");
IConfigurationRoot Configuration = configurationBuilder.Build();
string _portName = Configuration.GetSection("COMPort").Exists() ? Configuration.GetSection("COMPort").Value : "COM1";
int _baudRate = Configuration.GetSection("BaudRate").Exists() ? Int32.Parse(Configuration.GetSection("BaudRate").Value) : 9600;

TeamsStatus teamsStatus = new TeamsStatus();
Presence previousPresence = new Presence();
Presence presence = new Presence();

Indicator _indicator = new Indicator(_portName, _baudRate);
bool previousIndicatorState = false;
bool indicatorState = _indicator.getIndicatorState();

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

    previousIndicatorState = (indicatorState ? true : false);
    indicatorState = _indicator.getIndicatorState();

    presence = (Presence)teamsStatus.getPresence();
    Console.WriteLine("Availability: " + presence.availability);

    //_indicator.SetIndicator(colors.ToArray()[_counter % colors.Count].Value); // Debug: Cycle through colors
    if (!indicatorState) {
        _indicator.Reconnect();
    }
    if (!presence.Equals(previousPresence) || (previousIndicatorState == false && indicatorState == true)) { // Presence has changed.
        IndicatorInstruction color;
        indicatorInstructionMapping.TryGetValue(presence.availability, out color);
        _indicator.SetIndicator(color);
        previousPresence.setPresence(presence);
    }

    _counter = (_counter + 1) % indicatorInstructionMapping.Count;

    autoEvent.Reset();

}


