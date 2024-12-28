using System.IO.Ports;

namespace TeamsStatusLight
{
    internal interface IIndicator
    {
        void SetIndicator(IIndicatorInstruction instruction);
        void Write(string input);
        void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e);
        void DataReceivedCallback(string response);
        void StopAllSerial();
        void StartSerial();

    }
}
