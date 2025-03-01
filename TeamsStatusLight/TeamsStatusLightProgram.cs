using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TeamsStatusLight
{
    public class TeamsStatusLightProgram
    {
        TeamsStatus teamsStatus;
        Presence previousPresence;
        Presence presence;
        ConfigurationBuilder configurationBuilder;
        IConfigurationRoot Configuration;
        string _portName;
        int _baudRate;
        Indicator _indicator;
        bool previousIndicatorState;
        bool indicatorState;
        Dictionary<string, IndicatorInstruction> indicatorInstructionMapping;
        IndicatorInstruction currentInstruction;
        IndicatorInstruction newInstruction;
        AutoResetEvent autoEvent;
        Timer t;

        public TeamsStatusLightProgram() {
            teamsStatus = new TeamsStatus();
            previousPresence = new Presence();
            presence = new Presence();
            configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            Configuration = configurationBuilder.Build();
            _portName = Configuration.GetSection("COMPort").Exists() ? Configuration.GetSection("COMPort").Value : "COM1";
            _baudRate = Configuration.GetSection("BaudRate").Exists() ? Int32.Parse(Configuration.GetSection("BaudRate").Value) : 9600;
            _indicator = new Indicator(_portName, _baudRate);
            previousIndicatorState = false;
            indicatorState = _indicator.getIndicatorState();

            indicatorInstructionMapping = IndicatorInstruction.DeserializeIndicatorInstructionsFromConfig(Configuration.GetSection("IndicatorInstructionMapping"));

            IndicatorInstruction currentInstruction;
            IndicatorInstruction newInstruction;

            autoEvent = new AutoResetEvent(false);
            t = new Timer(CheckPresenceAndSetIndicator, autoEvent, 0, 5000);
            //autoEvent.WaitOne();
        }
        static void Main(string[] args)
        {
            TeamsStatusLightProgram teamsStatusLight = new TeamsStatusLightProgram();
            teamsStatusLight.Run();
        }
        public void Run()
        {
            //TeamsStatus teamsStatus = new TeamsStatus();
            //Presence previousPresence = new Presence();
            //Presence presence = new Presence();
            //ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            //configurationBuilder.AddJsonFile("appsettings.json");
            //IConfigurationRoot Configuration = configurationBuilder.Build();
            //string _portName = Configuration.GetSection("COMPort").Exists() ? Configuration.GetSection("COMPort").Value : "COM1";
            //int _baudRate = Configuration.GetSection("BaudRate").Exists() ? Int32.Parse(Configuration.GetSection("BaudRate").Value) : 9600;
            //Indicator _indicator = new Indicator(_portName, _baudRate);
            //bool previousIndicatorState = false;
            //bool indicatorState = _indicator.getIndicatorState();

            //Dictionary<string, IndicatorInstruction> indicatorInstructionMapping = IndicatorInstruction.DeserializeIndicatorInstructionsFromConfig(Configuration.GetSection("IndicatorInstructionMapping"));

            //int _counter = 0;

            //IndicatorInstruction currentInstruction;
            //IndicatorInstruction newInstruction;

            //autoEvent = new AutoResetEvent(false);
            //t = new Timer(CheckPresenceAndSetIndicator, autoEvent, 0, 5000);
            autoEvent.WaitOne();

        }
        void CheckPresenceAndSetIndicator(Object stateinfo)
        {
            Console.WriteLine("Invoked CheckPresenceAndSetIndicator");
            AutoResetEvent autoEvent = (AutoResetEvent)stateinfo;

            previousIndicatorState = (indicatorState ? true : false);
            indicatorState = _indicator.getIndicatorState();

            presence = (Presence)teamsStatus.getPresence();
            Console.WriteLine("Availability: " + presence.availability);

            if (!indicatorState)
            {
                _indicator.Reconnect();
            }
            if (!presence.Equals(previousPresence) || (previousIndicatorState == false && indicatorState == true))
            { // Presence has changed.
                IndicatorInstruction color;
                indicatorInstructionMapping.TryGetValue(presence.availability, out color);
                _indicator.SetIndicator(color);
                previousPresence.setPresence(presence);
            }
            autoEvent.Reset();

        }

        public void EnterSuspendState()
        {
            _indicator.EnterSuspendState();
        }
    }
}