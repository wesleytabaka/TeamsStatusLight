using System.IO.Ports;

namespace TeamsStatusLight
{
    public class Indicator : IIndicator
    {
        SerialPort _serialPort;
        bool _continue;

        public Indicator(string portName) { 
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = 9600;
            _serialPort.DtrEnable = true;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            StartSerial();
        }

        public void StartSerial() {
            try
            {
                _serialPort.Open();
                _continue = true;
            }
            catch (Exception ex) {
                _continue = false;
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public void StopAllSerial() {
            if (_serialPort.IsOpen) {
                _serialPort.Close();
            }
            this._continue = false;
        }

        public void SetIndicator(IIndicatorInstruction instruction)
        {
            Write(String.Join(",", Convert.ChangeType(instruction.transition, instruction.transition.GetTypeCode()), instruction.transitionDuration, Convert.ChangeType(instruction.effect, instruction.effect.GetTypeCode()), instruction.effectRate, instruction.r, instruction.g, instruction.b, instruction.r2, instruction.g2, instruction.b2, "-"));
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadExisting();
            DataReceivedCallback(data);
        }

        public void DataReceivedCallback(string message) {
            Console.WriteLine("RX: " + message);
        }

        public void Write(string input) {
            if (_continue)
            {
                _serialPort.WriteLine(input);
            }
        }
    }
}
